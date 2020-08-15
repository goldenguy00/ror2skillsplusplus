using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

using RoR2;
using RoR2.UI;

using Rewired;
using Rewired.Data;
using Rewired.Data.Mapping;

using R2API.Utils;

namespace SkillsPlusPlus {
    static class SkillInput {

        public const int BUY_SKILLS_ACTION_ID = 400;
        public const string BUY_SKILLS_ACTION_NAME = "BuySkills";
        private const string UI_TOKEN = "Buy Skills"; //"SKILLS_GAMEPAD_BUY_BTN";
        private const string UI_HOVER_TOKEN = "Holding this button during gameplay will allow the purchase of skills. Once held press the button for the skill you want to purchase."; // "SKILLS_GAMEPAD_BUY_BTN";

        internal static void SetupCustomInput() {
            On.Rewired.ReInput.KFIfLMJhIpfzcbhqEXHpaKpGsgeZ += SkillInput.ReInput_KFIfLMJhIpfzcbhqEXHpaKpGsgeZ;
            SceneManager.activeSceneChanged += OnFirstSceneLoad;

            On.RoR2.UI.SettingsPanelController.Start += (orig, self) => {
                SetupGamepadSettingsControllerAwake(self);
                orig(self);
            };
        }

        private static void OnFirstSceneLoad(Scene _, Scene __) {
            SceneManager.activeSceneChanged -= OnFirstSceneLoad;
            // attempt to create new input item in catalog
            try {
                InputCatalog.GetActionNameToken(BUY_SKILLS_ACTION_NAME);
            } catch {
                try {
                    Type actionAxisPairType = Reflection.GetNestedTypeCached(typeof(InputCatalog), "ActionAxisPair");
                    var actionAxisPair = actionAxisPairType.GetConstructorCached(new Type[] { typeof(string), typeof(AxisRange) }).Invoke(new object[] { BUY_SKILLS_ACTION_NAME, AxisRange.Full });
                    typeof(InputCatalog).GetFieldValue<System.Collections.IDictionary>("actionToToken").Add(actionAxisPair, UI_TOKEN);
                } catch(Exception exception) {
                    Logger.Error(exception);
                    return;
                }
            }
        }

        private static void ReInput_KFIfLMJhIpfzcbhqEXHpaKpGsgeZ(On.Rewired.ReInput.orig_KFIfLMJhIpfzcbhqEXHpaKpGsgeZ orig, InputManager_Base inputManager, Func<ConfigVars, object> configVarsFunc, ConfigVars configVars, ControllerDataFiles controllerDataFiles, UserData userData) {

            int newActionId = userData.GetFieldValue<int>("actionIdCounter");
            SkillsPlusPlus.Logger.Warn(newActionId);
            // when the action is created by this method the actual ID of the action will not be this value
            // the newActionId captured prior will be the actual internal ID of the action
            userData.InsertAction(0, BUY_SKILLS_ACTION_ID);

            // thus we locate it by its actual ID and update the values and its description via reflection
            InputAction inputAction = userData.GetActionById(newActionId);
            inputAction.SetPropertyValue("id", BUY_SKILLS_ACTION_ID);
            inputAction.SetPropertyValue("name", BUY_SKILLS_ACTION_NAME);

            userData.ChangeActionCategory(BUY_SKILLS_ACTION_ID, 2); // ensures that the category is aware of the new action
            userData.ChangeActionCategory(BUY_SKILLS_ACTION_ID, 0); // ensures that the category is aware of the new action


            ControllerMap_Editor joystickEditor = userData.GetJoystickMapById(0, out int joystickIndex);
            ActionElementMap purchaseSkillActionElementMap = new ActionElementMap(BUY_SKILLS_ACTION_ID, ControllerElementType.Button, 0);
            joystickEditor.actionElementMaps.Add(purchaseSkillActionElementMap);

            SkillsPlusPlus.Logger.Warn("Delegating to original ReInput method");
            orig(inputManager, configVarsFunc, configVars, controllerDataFiles, userData);
        }

        private static void SetupGamepadSettingsControllerAwake(SettingsPanelController settingsPanelController) {
            if(settingsPanelController.name != "SettingsSubPanel, Controls (Gamepad)") {
                return;
            }
            if(InputCatalog.GetActionNameToken(BUY_SKILLS_ACTION_NAME) == null) {
                return;
            }
            if(settingsPanelController.TryGetComponentInChildren(out InputBindingControl existingInputBindingController)) {
                GameObject buySkillsOptionGameObject = GameObject.Instantiate(existingInputBindingController.gameObject, existingInputBindingController.gameObject.transform.parent);
                buySkillsOptionGameObject.name = "SettingsEntryButton, Binding (Skills++)";
                InputBindingControl inputBindingControl = buySkillsOptionGameObject.GetComponent<InputBindingControl>();
                //inputBindingControl.nameLabel.token = "SKILLS++_TOKEN";
                inputBindingControl.actionName = BUY_SKILLS_ACTION_NAME;
                inputBindingControl.Awake();

                HGButton button = buySkillsOptionGameObject.GetComponent<HGButton>();
                button.hoverToken = UI_HOVER_TOKEN;

            }

        }
    }
}
