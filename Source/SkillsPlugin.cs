using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using RoR2.ContentManagement;
using RoR2.UI;

using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.UI;
using SkillsPlusPlus.Util;
using UnityEngine;
using UnityEngine.Networking;
using LoadoutPanelController = On.RoR2.UI.LoadoutPanelController;

namespace SkillsPlusPlus {

    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.KingEnderBrine.ExtendedLoadout", BepInDependency.DependencyFlags.SoftDependency)] //Soft-dependency to make Skills++ load after ExtendedLoadout
    [BepInPlugin("com.cwmlolzlz.skills", "Skills", "0.6.3")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod)]
    public sealed class SkillsPlugin : BaseUnityPlugin {

        static SkillsPlugin Instance = null;

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

            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };

            // On.RoR2.Console.RunCmd += (orig, self, sender, cmd, userArgs) => {
            //     SkillsPlusPlus.Logger.Warn(cmd);
            //     orig(self, sender, cmd, userArgs);
            // };

            //bool didAttemptToConnect = false;

            // disable client authing when connecting to a server to allow two game instances to run in parallel
            //On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };
            // On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
            //     if (didAttemptToConnect == false) {
            //         didAttemptToConnect = true;
            //         RoR2.Console.instance.SubmitCmd(null, "connect 192.168.1.102:27015", true);
            //     }
            //     orig(self);
            // };
            #endif
            Instance = this;

            CommandHelper.AddToConsoleWhenReady();

            R2API.RecalculateStatsAPI.GetStatCoefficients += LunarModifiers.RecalculateStats_GetLunarStats;

            //On.RoR2.Skills.SkillDef.CanExecute += SkillInput.GenericSkill_CanExecute;

            SkillModifierManager.LoadSkillModifiers();
            //SkillInput.SetupCustomInput();
            SkillOptions.SetupGameplayOptions();

            GameObject playerMasterPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/charactermasters/CommandoMaster");
            playerMasterPrefab.EnsureComponent<SkillPointsController>();

            LegacyResourcesAPI.Load<GameObject>("Prefabs/UI/Tooltip").EnsureComponent<SkillsPlusPlusTooltipController>();
            On.RoR2.UI.TooltipController.SetTooltipProvider += (orig, self, provider) => {
                orig(self, provider);
                if (provider.TryGetComponent(out SkillUpgradeTooltipProvider tooltipProvider)) {
                    var tooltipController = self.EnsureComponent<SkillsPlusPlusTooltipController>();
                    tooltipController.skillUpgradeToken = tooltipProvider.GetToken();
                }
            };
            On.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot += RowOnFromSkillSlot;

            object RowOnFromSkillSlot(LoadoutPanelController.Row.orig_FromSkillSlot orig, RoR2.UI.LoadoutPanelController owner, BodyIndex bodyindex, int skillslotindex, GenericSkill skillslot)
            {
                RoR2.UI.LoadoutPanelController.Row row = orig(owner, bodyindex, skillslotindex, skillslot) as RoR2.UI.LoadoutPanelController.Row;
                //var buttons = row.GetFieldValue<List<MPButton>>("buttons");
                if (row != null)
                {
                    var buttons = row.rowData;
                    SkillsPlusPlus.Logger.Debug("row " + buttons);
                    for (int i = 0; i < buttons.Count; i++) {
                        SkillsPlusPlus.Logger.Debug("Ensuring SkillsPlusPlusTooltipProvider({0})", i);
                        var button = buttons[i];
                        var skillDef = skillslot?.skillFamily?.variants[i].skillDef;
                        if (skillDef != null) {
                            var provider = button.button.gameObject.EnsureComponent<SkillUpgradeTooltipProvider>();
                            provider.skillName = ((ScriptableObject)skillDef)?.name;
                            SkillsPlusPlus.Logger.Debug(((ScriptableObject)skillDef)?.name);
                        }
                    }
                }
                else
                {
                    SkillsPlusPlus.Logger.Debug("row null ");
                }

                return row;
            }

            On.RoR2.UI.HUD.Awake += this.HUD_Awake;

            initConfig();
            
            SkillsPlusPlus.Logger.Debug("Awake() SurvivorCatalog.allSurvivorDef: {0}", SurvivorCatalog.allSurvivorDefs);
        }

        private void initConfig()
        {
            var levelsPerSkillPoint = Config.Bind("Skills++",
                "Levels per skill point",
                5f,
                "The number of levels to reach to be rewarded with a skillpoint. Changes will not be applied during a run. In multiplayer runs the host's setting is used");

            SliderConfig slider = new SliderConfig
            {
                max = 50,
                min = 1,
                FormatString = "{0:0}"
            };
            
            ModSettingsManager.AddOption(new SliderOption(levelsPerSkillPoint, slider));

            levelsPerSkillPoint.SettingChanged += (sender, args) =>
            {
                ConVars.ConVars.levelsPerSkillPoint.value = Mathf.RoundToInt(levelsPerSkillPoint.Value);
            };
            
            var skillActionName = Config.Bind("Skills++",
                "Keybind to upgrade skills",
                KeyboardShortcut.Empty,
                "Key to upgrade skills. When a skill is available to be upgraded, holding the key down and pressing the associated skill key will upgrade the skill.");
            
            ModSettingsManager.AddOption(new KeyBindOption(skillActionName));

            skillActionName.SettingChanged += (sender, args) =>
            {
                ConVars.ConVars.buySkillsKeybind = skillActionName.Value;
                SkillsPlusPlus.Logger.Debug(skillActionName.Value.MainKey.ToString());
            };
            
            var disableInput = Config.Bind("Skills++",
                "Disable Skills While Buying",
                true,
                "Should skills be disabled while the Buy Skills Input is pressed. (Disable this if you find yourself hitting the key by mistake)");
            
            disableInput.SettingChanged += (sender, args) =>
            {
                ConVars.ConVars.disableOnBuy.value = disableInput.Value;
            };
            
            ModSettingsManager.AddOption(new CheckBoxOption(disableInput));
            
            
            
            var multScalingLinear = Config.Bind("Skills++",
                "Linear Skill Multipliers",
                false,
                "Should Multiplicative (+%) skill values use a linear value rather than an exponential one. (Useful for playing with low \"Levels per skill point\" values). In multiplayer runs the host's setting is used");
            
            multScalingLinear.SettingChanged += (sender, args) =>
            {
                ConVars.ConVars.multScalingLinear.value = multScalingLinear.Value;
            };
            
            ModSettingsManager.AddOption(new CheckBoxOption(multScalingLinear));

            ConVars.ConVars.buySkillsKeybind = skillActionName.Value;
            ConVars.ConVars.levelsPerSkillPoint.value = Mathf.RoundToInt(levelsPerSkillPoint.Value);
            ConVars.ConVars.disableOnBuy.value = disableInput.Value;
            ConVars.ConVars.multScalingLinear.value = multScalingLinear.Value;
        }

        [SystemInitializer(typeof(SurvivorCatalog))]
        private static void OnCatalogInitializer()
        {
            foreach (var survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                Instance.PrepareSurvivor(survivorDef);
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