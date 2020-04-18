using BepInEx;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using R2API.Utils;

namespace Skills {
    
    [BepInDependency ("com.bepis.r2api")]
    [BepInPlugin ("com.cwmlolzlz.skills", "Skills", "0.0.1")]

    public class PhotoModePlugin : BaseUnityPlugin {

        private HUD hud;
        private CharacterBody body;

        private SkillPointsController skillsController;
        private SkillLevelIconController[] skillsHUDControllers;

        

        public void Awake () {

            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };

            // On.Skill
            On.RoR2.PlayerCharacterMasterController.SetBody += (orig, self, bodyGameObject) => {
                orig(self, bodyGameObject);
                if(bodyGameObject) {
                    this.body = bodyGameObject.GetComponent<CharacterBody>();
                    TryCreateSkillsController();
                }
            };

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                orig(self);
                SkillPointsController skillsController = self.GetComponent<SkillPointsController>();
                if(skillsController) {
                    skillsController.OnLevelChanged();
                }
            };

            On.RoR2.UI.HUD.Awake += (orig, self) => {
                orig(self);
                this.hud = self;
                TryCreateSkillsController();
            };

        }

        private void TryCreateSkillsController() {
            if(skillsController == null) {
                this.skillsController = body.gameObject.AddComponent<SkillPointsController>();
            }
            skillsController.SetCharacterBody(body);

            if(hud && hud.mainUIPanel) {

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
                skillsController.SetSkillIconControllers(null);
            }
        }

    }
}