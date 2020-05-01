using BepInEx;
using RoR2;
using RoR2.UI;
using UnityEngine;
using R2API.Utils;

namespace Skills {
    
    [BepInDependency ("com.bepis.r2api")]
    [BepInPlugin ("com.cwmlolzlz.skills", "Skills", "0.0.1")]
    [R2APISubmoduleDependency("AssetPlus")]
    public class PhotoModePlugin : BaseUnityPlugin {

        private HUD hud;
        private PlayerCharacterMasterController playerCharacterMasterController;

        //private SkillPointsController skillsController;
        //private SkillLevelIconController[] skillsHUDControllers;

        public void Awake () {

#if DEBUG
            // disable client authing when connecting to a server to allow two game instances to run in parallel
            On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };
#endif

            SkillModifierManager.LoadSkillModifiers();

#if DEBUG
            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };
            On.RoR2.CharacterMaster.Awake += (orig, self) => {
                orig(self);
                if(self.GetFieldValue<bool>("godMode") == false){
                    self.InvokeMethod("ToggleGod");
                }
            }; // god mode for all
#endif

            On.RoR2.PlayerCharacterMasterController.Awake += (orig, self) => {
                orig(self);
                self.master.onBodyStart += _ => {
                    Debug.LogFormat("{0} hasEffectiveAuthority {1}", self, self.hasEffectiveAuthority);
                    Debug.LogFormat("{0} hasAuthority {1}", self, self.hasAuthority);
                    Debug.LogFormat("{0} isLocalPlayer {1}", self, self.isLocalPlayer);
                    Debug.LogFormat("{0} isClient {1}", self, self.isClient);
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