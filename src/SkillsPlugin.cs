using BepInEx;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using R2API.AssetPlus;
using R2API.Utils;
using UnityEngine;
using SkillsPlusPlus.Modifiers;

namespace SkillsPlusPlus {
    
    [BepInDependency ("com.bepis.r2api")]
    [BepInPlugin ("com.cwmlolzlz.skills", "Skills", "0.0.11")]
    // [R2APISubmoduleDependency("AssetPlus")]
    public class SkillsPlusPlusPlugin : BaseUnityPlugin {

        private HUD hud;
        private PlayerCharacterMasterController playerCharacterMasterController;

        //private SkillPointsController skillsController;
        //private SkillLevelIconController[] skillsHUDControllers;

        public void Awake () {

            LoaderKnucklesSkillModifier.PatchSkillName();
            LoaderThrowPylonSkillModifier.PatchSkillName();

#if DEBUG
            // disable client authing when connecting to a server to allow two game instances to run in parallel
            On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };
#endif

            SkillModifierManager.LoadSkillModifiers();

#if DEBUG
            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };
            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };

            On.RoR2.CombatDirector.OnEnable += (orig, self) => {
                orig(self);
                self.creditMultiplier = 10;    
            };

#endif

            On.RoR2.PlayerCharacterMasterController.Awake += (orig, self) => {
                orig(self);
                self.master.onBodyStart += _ => {
#if DEBUG
                    if(self.master.GetBody().healthComponent.godMode == false){
                        self.master.GetBody().healthComponent.godMode = true;
                    }

#endif
                    if (self.hasEffectiveAuthority) {
                        this.playerCharacterMasterController = self;
                        TryCreateSkillsController();
                    }
                };
            };

            On.RoR2.UI.CharacterSelectController.Awake += (orig, self) => {
                orig(self);
                if (self.gameObject.GetComponent<SkillModifierTooltipController>() == null) {
                    SkillModifierTooltipController skillModifierTooltipController = self.gameObject.AddComponent<SkillModifierTooltipController>();
                }
            };

            On.RoR2.UI.HUD.Awake += (orig, self) => {
                orig(self);
                this.hud = self;
                TryCreateSkillsController();
            };

        }

        public void Update() {
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
                SkillPointsController skillsController;
                if (playerCharacterMasterController.gameObject.TryGetComponent(out skillsController) == false) {
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
                //skillsController.SetSkillIconControllers(null);
            }
        }

    }
}