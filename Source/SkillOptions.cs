using System;
using System.Collections.Generic;

using RoR2;
using RoR2.UI;
using RoR2.ConVar;
using UnityEngine;

namespace SkillsPlusPlus {
    class SkillOptions {

        private static GameObject carouselPrefab;
        private static GameObject boolPrefab;
        private static Transform gameplaySettingsPanelTransform;
        private static CarouselController levelsPerSkillPointCarousel;
        private static CarouselController multLinearScaleCarousel;

        internal static void SetupGameplayOptions() {
            On.RoR2.UI.SettingsPanelController.Start += (orig, self) => {
                orig(self);
                SettingsPanelControllerAwake(self);
            };
        }

        private static void SettingsPanelControllerAwake(SettingsPanelController settingsPanelController) {
            if(!levelsPerSkillPointCarousel) {

                if(settingsPanelController.name == "SettingsSubPanel, Gameplay") {
                    Logger.Debug("Got gameplay controller");
                    gameplaySettingsPanelTransform = settingsPanelController.GetComponentInChildren<BaseSettingsControl>(true).transform.parent;
                    var carouselControllers = settingsPanelController.transform.parent.GetComponentsInChildren<CarouselController>(true);
                    Logger.Debug(carouselControllers.Length);
                    carouselPrefab = Array.Find(carouselControllers, carouselController => {
                        return carouselController.leftArrowButton != null || carouselController.rightArrowButton != null;
                    })?.gameObject;
                }

                if(gameplaySettingsPanelTransform != null && carouselPrefab != null) {
                    Logger.Debug("Adding option");
                    GameObject gameObject = GameObject.Instantiate(carouselPrefab, gameplaySettingsPanelTransform);
                    gameObject.name = "SettingsEntryButton, Carousel (Skills++)";
                    levelsPerSkillPointCarousel = gameObject.GetComponent<CarouselController>();
                    levelsPerSkillPointCarousel.forceValidChoice = false;
                    levelsPerSkillPointCarousel.settingSource = BaseSettingsControl.SettingSource.ConVar;
                    levelsPerSkillPointCarousel.settingName = ConVars.ConVars.levelsPerSkillPoint.name;
                    levelsPerSkillPointCarousel.nameToken = "LEVELS_PER_SKILLPOINT";
                    levelsPerSkillPointCarousel.nameLabel.token = "LEVELS_PER_SKILLPOINT";
                    levelsPerSkillPointCarousel.GetComponent<HGButton>().hoverToken = ConVars.ConVars.levelsPerSkillPoint.helpText;

                    var choices = new List<CarouselController.Choice>();

                    for(int i = ConVars.ConVars.levelsPerSkillPoint.minValue; i <= ConVars.ConVars.levelsPerSkillPoint.maxValue; i++) {
                        choices.Add(new CarouselController.Choice() {
                            convarValue = "" + i,
                            suboptionDisplayToken = "" + i
                        }) ;
                    }

                    levelsPerSkillPointCarousel.choices = choices.ToArray();

                    // triggers a OnEnable call that will revalidate the controls
                    levelsPerSkillPointCarousel.enabled = false;
                    levelsPerSkillPointCarousel.enabled = true;


                    //HGButton button = buySkillsOptionGameObject.GetComponent<HGButton>();
                    //button.hoverToken = UI_HOVER_TOKEN;
                    //button.interactable = SkillInput.isControllerSupported;
                    //inputBindingControl.button.enabled = SkillInput.isControllerSupported;
                }
            }

            if(!multLinearScaleCarousel) { 
                if (settingsPanelController.name == "SettingsSubPanel, Gameplay")
                {
                    foreach(BaseSettingsControl settings in settingsPanelController.settingsControllers)
                    {
                        if(settings.name.Contains(", Bool"))
                        {
                            boolPrefab = settings.gameObject;
                            break;
                        }
                    }
                }
                if (boolPrefab)
                {
                    GameObject gameObject1 = GameObject.Instantiate(boolPrefab, gameplaySettingsPanelTransform);
                    gameObject1.name = "SettingsEntryButton, Bool (LinearSkill - Skills++)";
                    multLinearScaleCarousel = gameObject1.GetComponent<CarouselController>();
                    multLinearScaleCarousel.forceValidChoice = false;
                    multLinearScaleCarousel.settingSource = BaseSettingsControl.SettingSource.ConVar;
                    multLinearScaleCarousel.settingName = ConVars.ConVars.multScalingLinear.name;
                    multLinearScaleCarousel.nameToken = "MULT_SCALING_LINEAR";
                    multLinearScaleCarousel.nameLabel.token = "MULT_SCALING_LINEAR";
                    multLinearScaleCarousel.GetComponent<HGButton>().hoverToken = ConVars.ConVars.multScalingLinear.helpText;

                    multLinearScaleCarousel.enabled = false;
                    multLinearScaleCarousel.enabled = true;
                }
            }
        }
    }
}
