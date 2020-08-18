
using System;
using BepInEx;
using UnityEngine;

using RoR2;
using RoR2.UI;
using R2API.Utils;
using SkillsPlusPlus.Modifiers;
using RoR2.ConVar;

namespace SkillsPlusPlus {
    
    [BepInDependency ("com.bepis.r2api")]
    [BepInPlugin ("com.cwmlolzlz.skills", "Skills", "0.1.1")]
    [R2APISubmoduleDependency(nameof(CommandHelper))]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync)]
    public sealed class SkillsPlugin : BaseUnityPlugin {

        private HUD hud;
        private PlayerCharacterMasterController playerCharacterMasterController;

        //private SkillPointsController skillsController;
        //private SkillLevelIconController[] skillsHUDControllers;

        void Awake() {

#if DEBUG
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.Debug;

            // disable client authing when connecting to a server to allow two game instances to run in parallel
            // On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };

            // BoolConVar convar = typeof(IntroCutsceneController).GetFieldValue<BoolConVar>("cvIntroSkip");
            // convar.
            // Console.instance.SubmitCmd(null, "set_scene title", false);
            // RoR2.Console.instance.SubmitCmd(null, "set_scene title", false);
            On.RoR2.Console.InitConVars += (orig, self) => {
                orig(self);
                RoR2.Console.instance.SubmitCmd(null, "splash_skip 1", false);
                RoR2.Console.instance.SubmitCmd(null, "intro_skip 1", false);
            };

            // On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
            //    orig(self);
            //    // RoR2.Console.instance.SubmitCmd(null, "host 1", true);
            //    RoR2.Console.instance.SubmitCmd(null, "connect 127.0.0.1:27015", true);                
            // };

            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };
#endif

            CommandHelper.AddToConsoleWhenReady();

            LoaderKnucklesSkillModifier.PatchSkillName();
            LoaderThrowPylonSkillModifier.PatchSkillName();

            SkillModifierManager.LoadSkillModifiers();
            SkillInput.SetupCustomInput();

            On.RoR2.PlayerCharacterMasterController.Awake += (orig, self) => {
                orig(self);
                self.master.onBodyStart += _ => {
#if DEBUG
                    if(self.master.GetBody().healthComponent.godMode == false) {
                        self.master.GetBody().healthComponent.godMode = true;
                    }

#endif
                    if(self.hasEffectiveAuthority) {
                        this.playerCharacterMasterController = self;
                        TryCreateSkillsController();
                    }
                };
            };

            On.RoR2.UI.CharacterSelectController.Awake += (orig, self) => {
                orig(self);
                if(self.gameObject.GetComponent<SkillModifierTooltipController>() == null) {
                    SkillModifierTooltipController skillModifierTooltipController = self.gameObject.AddComponent<SkillModifierTooltipController>();
                }
            };

            On.RoR2.UI.HUD.Awake += (orig, self) => {
                orig(self);
                this.hud = self;
                TryCreateSkillsController();
            };

        }

        void Update() {
#if DEBUG
            if(Input.GetKeyDown(KeyCode.Keypad1) && playerCharacterMasterController != null && playerCharacterMasterController.master.GetBody() != null) {
                GameObject teleporter = GameObject.Find("Teleporter1(Clone)");
                Transform spawnLocation = playerCharacterMasterController.master.GetBody().transform;
                if(teleporter != null && teleporter.TryGetComponent(out TeleporterInteraction teleporterInteraction)) {
                    GameObject.Instantiate(teleporterInteraction.shopPortalSpawnCard.prefab, spawnLocation.position, spawnLocation.rotation, null);                                       
                }
            }
#endif
        }

        private void TryCreateSkillsController() {
            if (hud && hud.mainUIPanel && playerCharacterMasterController != null) {
                if(playerCharacterMasterController.gameObject.TryGetComponent(out SkillPointsController skillsController) == false) {
                    skillsController = playerCharacterMasterController.gameObject.AddComponent<SkillPointsController>();
                }

                SkillIcon[] skillIcons = hud.mainUIPanel.GetComponentsInChildren<SkillIcon>();
                SkillLevelIconController[] skillIconControllers = new SkillLevelIconController[skillIcons.Length];
                for(int i = 0; i < skillIcons.Length; i++) {
                    SkillLevelIconController existingSkillLevelIconController = skillIcons[i].GetComponent<SkillLevelIconController>();
                    if(existingSkillLevelIconController) {
                        skillIconControllers[i] = existingSkillLevelIconController;
                    } else {
                        skillIconControllers[i] = skillIcons[i].gameObject.AddComponent<SkillLevelIconController>();
                    }
                }
                skillsController.SetSkillIconControllers(skillIconControllers);
            } else {
                // skillsController.SetSkillIconControllers(null);
            }
        }

    }
}