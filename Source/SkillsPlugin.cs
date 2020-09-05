
using System;
using BepInEx;
using UnityEngine;

using RoR2;
using RoR2.UI;
using R2API;
using R2API.Utils;
using SkillsPlusPlus.Modifiers;
using RoR2.ConVar;
using R2API.AssetPlus;
using System.Linq;

namespace SkillsPlusPlus {
    
    [BepInDependency ("com.bepis.r2api")]
    [BepInPlugin ("com.cwmlolzlz.skills", "Skills", "0.1.5")]
    [R2APISubmoduleDependency(nameof(CommandHelper), nameof(LanguageAPI))]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
    public sealed class SkillsPlugin : BaseUnityPlugin {

        private SkillPointsController localSkillPointsController;
        private HUD hud;

        //private SkillPointsController skillsController;
        //private SkillLevelIconController[] skillsHUDControllers;

        void Awake() {

#if DEBUG
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.Debug;

            // disable client authing when connecting to a server to allow two game instances to run in parallel
            // On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };

            On.RoR2.Console.InitConVars += (orig, self) => {
                orig(self);
                RoR2.Console.instance.SubmitCmd(null, "splash_skip 1", false);
                RoR2.Console.instance.SubmitCmd(null, "intro_skip 1", false);
            };

            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };

            bool didAttemptToConnect = false;
            On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
                if(didAttemptToConnect == false) {
                    didAttemptToConnect = true;
                    RoR2.Console.instance.SubmitCmd(null, "connect 192.168.1.102:27015", true);
                }
                orig(self);
            };
#endif

            CommandHelper.AddToConsoleWhenReady();

            LoaderKnucklesSkillModifier.PatchSkillName();
            LoaderThrowPylonSkillModifier.PatchSkillName();

            SkillModifierManager.LoadSkillModifiers();
            SkillInput.SetupCustomInput();
            SkillOptions.SetupGameplayOptions();
            
            On.RoR2.PlayerCharacterMasterController.Awake += (orig, self) => {
                orig(self);
                SkillPointsController skillsController = self.gameObject.EnsureComponent<SkillPointsController>();
                if(self.hasEffectiveAuthority) {
                    this.localSkillPointsController = skillsController;
                    TryCreateSkillsController();
                }
            };

            //On.RoR2.UI.CharacterSelectController.Awake += (orig, self) => {
            //    orig(self);
            //    if(self.gameObject.GetComponent<SkillModifierTooltipController>() == null) {
            //        SkillModifierTooltipController skillModifierTooltipController = self.gameObject.AddComponent<SkillModifierTooltipController>();
            //    }
            //};

            // On.RoR2.UI.S

            On.RoR2.UI.HUD.Awake += (orig, self) => {
                orig(self);
                this.hud = self;
                TryCreateSkillsController();
            };

        }

        private void TryCreateSkillsController() {
            SkillsPlusPlus.Logger.Warn("TryCreateSkillsController {0} {1} {2}", this.hud, this.localSkillPointsController, hud?.skillIcons);
            if (hud && hud.skillIcons != null && this.localSkillPointsController != null) {
                SkillLevelIconController[] skillIconControllers = hud.skillIcons.Select(icon => {
                    return icon.EnsureComponent<SkillLevelIconController>();
                }).ToArray();
                this.localSkillPointsController.SetSkillIconControllers(skillIconControllers);
            } else {
                // skillsController.SetSkillIconControllers(null);
            }
        }

    }
}