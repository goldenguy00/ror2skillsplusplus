using BepInEx;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using R2API.AssetPlus;
using R2API.Utils;
using System.Net.NetworkInformation;
using Facepunch.Steamworks;
using Skills.Modifiers;

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

            SkillModifierManager.LoadSkillModifiers();

#if DEBUG
            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };
#endif

            On.RoR2.PlayerCharacterMasterController.Awake += (orig, self) => {
                orig(self);
                this.playerCharacterMasterController = self;
                TryCreateSkillsController();
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