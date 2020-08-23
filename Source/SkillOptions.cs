using System;
using System.Collections.Generic;

using RoR2;
using RoR2.UI;
using RoR2.ConVar;
using UnityEngine;

namespace SkillsPlusPlus {
    class SkillOptions {

        private static GameObject carouselPrefab;
        private static Transform gameplaySettingsPanelTransform;
        private static CarouselController levelsPerSkillPointCarousel;

        internal static void SetupGameplayOptions() {
            On.RoR2.UI.SettingsPanelController.Start += (orig, self) => {
                orig(self);
                SettingsPanelControllerAwake(self);
            };
        }

        private static void SettingsPanelControllerAwake(SettingsPanelController settingsPanelController) {
            if(levelsPerSkillPointCarousel != null) {
                return;
            }

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

    }
}
