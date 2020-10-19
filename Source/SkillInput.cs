using System;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;
using R2API.Utils;
using Rewired;
using Rewired.Data;
using Rewired.Data.Mapping;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkillsPlusPlus {
    static class SkillInput {

        public static bool isControllerSupported = false;

        public const int BUY_SKILLS_ACTION_ID = 400;
        public const string BUY_SKILLS_ACTION_NAME = "BuySkills";
        private const string UI_TOKEN = "SKILLS_GAMEPAD_BUY_BTN";
        private const string UI_HOVER_TOKEN = "SKILLS_GAMEPAD_BUY_DESCRIPTION";

        internal static void SetupCustomInput() {
            var userDataInit = typeof(UserData).GetMethod("KFIfLMJhIpfzcbhqEXHpaKpGsgeZ", BindingFlags.NonPublic | BindingFlags.Instance);
            if (userDataInit != null) {
                new Hook(userDataInit, (Action<Action<UserData>, UserData>) SkillInput.ReInput_KFIfLMJhIpfzcbhqEXHpaKpGsgeZ);
                isControllerSupported = true;
            } else {
                Logger.Error("Unable to add extra action to controller bindings. Was not able to find the method \"KFIfLMJhIpfzcbhqEXHpaKpGsgeZ\" in Rewired.UserData");
            }
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
                    var actionToTokenField = typeof(InputCatalog).GetFieldValue<IDictionary>("actionToToken");
                    if (actionToTokenField != null) {
                        actionToTokenField.Add(actionAxisPair, UI_TOKEN);
                    }
                } catch (Exception exception) {
                    Logger.Error(exception);
                    return;
                }
            }
        }

        private static void ReInput_KFIfLMJhIpfzcbhqEXHpaKpGsgeZ(Action<UserData> orig, UserData userData) {

            int newActionId = userData.GetFieldValue<int>("actionIdCounter");
            SkillsPlusPlus.Logger.Debug(newActionId);
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

            SkillsPlusPlus.Logger.Debug("Delegating to original ReInput method");
            orig(userData);
        }

        private static void SetupGamepadSettingsControllerAwake(SettingsPanelController settingsPanelController) {
            if (SkillInput.isControllerSupported == false) {
                return;
            }
            if (settingsPanelController.name != "SettingsSubPanel, Controls (Gamepad)") {
                return;
            }
            if (InputCatalog.GetActionNameToken(BUY_SKILLS_ACTION_NAME) == null) {
                return;
            }
            if (settingsPanelController.TryGetComponentInChildren(out InputBindingControl existingInputBindingController)) {
                GameObject buySkillsOptionGameObject = GameObject.Instantiate(existingInputBindingController.gameObject, existingInputBindingController.gameObject.transform.parent);
                buySkillsOptionGameObject.name = "SettingsEntryButton, Binding (Skills++)";
                InputBindingControl inputBindingControl = buySkillsOptionGameObject.GetComponent<InputBindingControl>();
                //inputBindingControl.nameLabel.token = "SKILLS++_TOKEN";
                inputBindingControl.actionName = BUY_SKILLS_ACTION_NAME;
                inputBindingControl.Awake();

                HGButton button = buySkillsOptionGameObject.GetComponent<HGButton>();
                button.hoverToken = UI_HOVER_TOKEN;
                button.interactable = SkillInput.isControllerSupported;
                inputBindingControl.button.enabled = SkillInput.isControllerSupported;
            }

        }
    }
}