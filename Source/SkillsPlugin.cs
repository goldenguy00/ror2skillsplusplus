using System.Collections.Generic;
using System.Collections.Specialized;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;

using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.UI;
using SkillsPlusPlus.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus {

    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin("com.cwmlolzlz.skills", "Skills", "0.2.2")]
    [R2APISubmoduleDependency(nameof(CommandHelper), nameof(LanguageAPI), nameof(SurvivorAPI), nameof(BuffAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod)]
    public sealed class SkillsPlugin : BaseUnityPlugin {

        void Awake() {

            //             SkillsPlusPlus.Logger.Warn(@"
            //   _____  _     _  _  _                       ____         _         
            //  / ____|| |   (_)| || |        _      _     |  _ \       | |        
            // | (___  | | __ _ | || | ___  _| |_  _| |_   | |_) |  ___ | |_  __ _ 
            //  \___ \ | |/ /| || || |/ __||_   _||_   _|  |  _ <  / _ \| __|/ _` |
            //  ____) ||   < | || || |\__ \  |_|    |_|    | |_) ||  __/| |_| (_| |
            // |_____/ |_|\_\|_||_||_||___/                |____/  \___| \__|\__,_|

            // Note: You are running the Skills++ {0} beta.
            // This is a pre-release and is to guarenteed to be stable, bug free, or crash free.

            // Raise bugs here https://discord.gg/wU94CjJ
            //             ", this.Info.Metadata.Version.ToString());

#if DEBUG
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.Debug;
            UnityEngine.Networking.LogFilter.currentLogLevel = LogFilter.Debug;

            On.RoR2.Console.InitConVars += (orig, self) => {
                orig(self);
                RoR2.Console.instance.SubmitCmd(null, "splash_skip 1", false);
                RoR2.Console.instance.SubmitCmd(null, "intro_skip 1", false);
            };

            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };

            // On.RoR2.Console.RunCmd += (orig, self, sender, cmd, userArgs) => {
            //     SkillsPlusPlus.Logger.Warn(cmd);
            //     orig(self, sender, cmd, userArgs);
            // };

            bool didAttemptToConnect = false;

            // disable client authing when connecting to a server to allow two game instances to run in parallel
            On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };
            // On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
            //     if (didAttemptToConnect == false) {
            //         didAttemptToConnect = true;
            //         RoR2.Console.instance.SubmitCmd(null, "connect 192.168.1.102:27015", true);
            //     }
            //     orig(self);
            // };
#endif

            CommandHelper.AddToConsoleWhenReady();

            LoaderKnucklesSkillModifier.PatchSkillName();
            LoaderThrowPylonSkillModifier.PatchSkillName();

            BanditSkillThrowSmokebombModifier.RegisterBanditSpeedBuff();

            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(BanditSkillSkullRevolverModifier.HealthComponent_TakeDamage);
            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(BanditSkillResetRevolverModifier.HealthComponent_TakeDamage);
            On.RoR2.CharacterBody.RecalculateStats += new On.RoR2.CharacterBody.hook_RecalculateStats(BanditSkillThrowSmokebombModifier.CharacterBody_RecalculateStats);

            SkillModifierManager.LoadSkillModifiers();
            SkillInput.SetupCustomInput();
            SkillOptions.SetupGameplayOptions();

            GameObject playerMasterPrefab = Resources.Load<GameObject>("prefabs/charactermasters/CommandoMaster");
            playerMasterPrefab.EnsureComponent<SkillPointsController>();

            Resources.Load<GameObject>("Prefabs/UI/Tooltip").EnsureComponent<SkillsPlusPlusTooltipController>();
            On.RoR2.UI.TooltipController.SetTooltipProvider += (orig, self, provider) => {
                orig(self, provider);
                if (provider.TryGetComponent(out SkillUpgradeTooltipProvider tooltipProvider)) {
                    var tooltipController = self.EnsureComponent<SkillsPlusPlusTooltipController>();
                    tooltipController.skillUpgradeToken = tooltipProvider.GetToken();
                }
            };
            On.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot += (orig, owner, bodyIndex, skillSlotIndex, genericSkill) => {
                object row = orig(owner, bodyIndex, skillSlotIndex, genericSkill);
                var buttons = row.GetFieldValue<List<MPButton>>("buttons");
                for (int i = 0; i < buttons.Count; i++) {
                    SkillsPlusPlus.Logger.Debug("Ensuring SkillsPlusPlusTooltipProvider({0})", i);
                    var button = buttons[i];
                    var skillDef = genericSkill?.skillFamily?.variants[i].skillDef;
                    if (skillDef != null) {
                        var provider = button.gameObject.EnsureComponent<SkillUpgradeTooltipProvider>();
                        provider.skillName = skillDef.skillName;
                    }
                }
                return row;
            };
            // LoadoutPanelController.loadoutButtonPrefab.EnsureComponent<SkillsPlusPlusTooltipProvider>();

            On.RoR2.UI.HUD.Awake += this.HUD_Awake;

            On.RoR2.SurvivorCatalog.Init += HookVanillaCharacters;
            SkillsPlusPlus.Logger.Debug("Awake() SurvivorCatalog.allSurvivorDef: {0}", SurvivorCatalog.allSurvivorDefs);
            SurvivorAPI.SurvivorDefinitions.CollectionChanged += OnSurvivorDefinitionsChanged;
        }

        private void HookVanillaCharacters(On.RoR2.SurvivorCatalog.orig_Init orig) {
            orig();
            foreach (var survivorDef in SurvivorCatalog.allSurvivorDefs) {
                PrepareSurvivor(survivorDef);
            }
        }

        private void OnSurvivorDefinitionsChanged(object sender, NotifyCollectionChangedEventArgs args) {
            foreach (var item in args.NewItems) {
                if (item is SurvivorDef survivorDef) {
                    PrepareSurvivor(survivorDef);
                }
            }
        }

        private void PrepareSurvivor(SurvivorDef survivorDef) {
            SkillsPlusPlus.Logger.Debug("PrepareSurvivor({0})", survivorDef);
            if (survivorDef == null) {
                return;
            }
            var skillUpgrades = survivorDef.bodyPrefab.GetComponents<SkillUpgrade>();
            skillUpgrades.ForEachTry(Destroy);

            if (survivorDef.bodyPrefab.TryGetComponent(out SkillLocator skillLocator)) {
                foreach (GenericSkill genericSkill in skillLocator.FindAllGenericSkills()) {
                    if (genericSkill == null) {
                        continue;
                    }
                    // if (SkillModifierManager.HasSkillModifier(genericSkill.baseSkill) == false) {
                    //     SkillsPlusPlus.Logger.Debug("SkillModifier for {0} does not exist", genericSkill.baseSkill);
                    //     continue;
                    // }

                    var skillUpgrade = survivorDef.bodyPrefab.AddComponent<SkillUpgrade>();

                    skillUpgrade.targetGenericSkill = genericSkill;
                    SkillsPlusPlus.Logger.Debug("+ {0}", genericSkill.skillName);
                }
            }
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self) {
            orig(self);
            self.GetComponentsInChildren<SkillIcon>(true).ForEachTry(skillIcon => {
                skillIcon.EnsureComponent<SkillUpgradeTooltipProvider>();
                skillIcon.EnsureComponent<SkillLevelIconController>();
            });
        }
    }
}