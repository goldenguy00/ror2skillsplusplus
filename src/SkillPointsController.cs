using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using EntityStates;
using R2API;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;

namespace Skills {

    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(SkillLocator))]
    class SkillPointsController : MonoBehaviour {

#if DEBUG
        private static int levelsPerPoint = 1;
        private static int skillLevelScaling = 2;
#else
        private static int levelsPerPoint = 5;
#endif

        private CharacterBody body;
        private SkillLocator skillLocator;

        private SkillLevelIconController[] skillIconControllers;

        //  private Dictionary<SkillSlot, int> spentSkillPoints;
        private Dictionary<SkillSlot, int> skillLevels;

        private int earnedSkillPoints = 0;
        private int unspentSkillPoints = 0;

        void Awake() {
            this.body = this.GetComponent<CharacterBody>();
            this.skillLocator = this.GetComponent<SkillLocator>();
            // this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillLevels = new Dictionary<SkillSlot, int>();
            foreach (SkillSlot value in Enum.GetValues(typeof(SkillSlot))) {
                skillLevels[value] = 1;
            }
            On.RoR2.EntityStateMachine.SetState += this.OnInterceptSetState;
        }

        void OnDestroy() {
            On.RoR2.EntityStateMachine.SetState -= this.OnInterceptSetState;
        }

        private void OnInterceptSetState(On.RoR2.EntityStateMachine.orig_SetState orig, EntityStateMachine self, EntityState newState) {
            if (newState != null && self.networker != null && self.networker.localPlayerAuthority) {
                if (self.commonComponents.characterBody == this.body) {
                    if (newState is BaseState) {
                        ISkillModifier skillModifier = SkillModifierManager.GetSkillModifierForEntityStateType(newState.GetType());
                        if (skillModifier != null && skillModifier.SkillDef?.skillName != null) {
                            GenericSkill genericSkill = this.skillLocator.FindSkill(skillModifier.SkillDef.skillName);
                            if (genericSkill != null) {
                                SkillSlot skillSlot = this.skillLocator.FindSkillSlot(genericSkill);
                                if (skillSlot != null) {
                                    Debug.LogFormat("Successfully intercepted entity state {1} for skill named {0}", skillModifier.SkillDef.skillName, newState.GetType().Name);
                                    this.EnsureSkillModifiersAreInitialised();
                                    skillModifier.OnSkillWillBeUsed((BaseState)newState, this.skillLevels[skillSlot]);
                                } else {
                                    Debug.LogErrorFormat("Could not identify skill slot for generic skill {0}", genericSkill);
                                }
                            } else {
                                Debug.LogErrorFormat("Could not find generic skill instance for skill named {0}", skillModifier.SkillDef.skillName);
                            }
                        } else {

                        }
                    } else {
                        Debug.Log(newState);
                    }
                } else {

                }
            }

            orig(self, newState);
        }

        private bool skillModifiersAreInitialised = false;
        private void EnsureSkillModifiersAreInitialised() {
            if (skillModifiersAreInitialised) {
                return;
            }
            skillModifiersAreInitialised = true;
            foreach (SkillSlot skillSlot in Enum.GetValues(typeof(SkillSlot))) {
                GenericSkill genericSkill = this.skillLocator.GetSkill(skillSlot);
                SkillDef skillDef = this.skillLocator.GetSkill(skillSlot).skillDef;
                ISkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);
                modifier.SkillDef = skillDef;
                modifier.OnSkillLeveledUp(skillLevels[skillSlot]);
            }
        }

        public void SetSkillIconControllers(SkillLevelIconController[] skillIconControllers) {
            this.skillIconControllers = skillIconControllers;
            if (skillIconControllers != null) {
                foreach (SkillLevelIconController skillIconController in skillIconControllers) {
                    skillIconController.OnBuy += this.OnBuySkill;
                }
            }
        }

        void Update() {

            EnsureSkillModifiersAreInitialised();

            bool infoButtonDown = body.master?.playerCharacterMasterController?.networkUser?.inputPlayer?.GetButton(RewiredConsts.Action.Info) == true;
            foreach(SkillLevelIconController skillLevelIconController in skillIconControllers) {
                skillLevelIconController.ShowBuyButton(infoButtonDown);
            }

#if DEBUG
            if(Input.GetKey(KeyCode.Equals)) {
                TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong) (500 * Time.deltaTime));
            }
#endif

        }

        private void OnBuySkill(SkillSlot skillSlot) {
            if (unspentSkillPoints <= 0) {
                return;
            }
            Debug.LogFormat("OnBuySkill({0})", Enum.GetName(typeof(SkillSlot), skillSlot));

            if (skillLevels.TryGetValue(skillSlot, out int skillLevel)) {
                SkillDef skillDef = body.skillLocator.GetSkill(skillSlot).skillDef;
                ISkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);

                if (skillLevel >= modifier.MaxLevel) {
                    return;
                }

                unspentSkillPoints--;


                // increment and store the new skill level
                skillLevels[skillSlot] = ++skillLevel;
                Debug.LogFormat("SkillSlot {0} @ level {1}", Enum.GetName(typeof(SkillSlot), skillSlot), skillLevel);

                // find an notify the modifer to update the skill's parameters
                if (modifier != null)
                {
                    modifier.OnSkillLeveledUp(skillLevel + 1);
                    body.skillLocator.GetSkill(skillSlot).RecalculateValues();
                }
                RefreshIconControllers();
            }
        }

        public void OnLevelChanged() {
            if(body == null && body.teamComponent) {
                Debug.LogError("body or body.teamComponent was null");
                return;
            }

            int characterLevel = (int) TeamManager.instance.GetTeamLevel(body.teamComponent.teamIndex);
            Debug.LogFormat("OnLevelChanged(%d)", characterLevel);

            int newSkillPoints = Math.Max(0, SkillPointsAtLevel(characterLevel) - earnedSkillPoints);

            earnedSkillPoints += newSkillPoints;
            unspentSkillPoints += newSkillPoints;

            RefreshIconControllers();
        }

        private void RefreshIconControllers() {
            if(skillIconControllers != null) {
                int characterLevel = (int) TeamManager.instance.GetTeamLevel(body.teamComponent.teamIndex);
                foreach(SkillLevelIconController skillLevelIconController in skillIconControllers) {

                    SkillSlot slot = skillLevelIconController.SkillSlot;
                    if(skillLevels.TryGetValue(slot, out int currentSkillLevel)) {
                        ISkillModifier modifier = SkillModifierManager.GetSkillModifier(body.skillLocator.GetSkill(slot).skillDef);
                    
                        int requiredLevelToBuySkill = (currentSkillLevel / skillLevelScaling);
                        // has skillpoints to spend
                        // meets required character level
                        // and the skill is less than its max level
                        skillLevelIconController.SetCanUpgrade(unspentSkillPoints > 0 && characterLevel >= requiredLevelToBuySkill && currentSkillLevel < modifier.MaxLevel);
                        skillLevelIconController.SetLevel(currentSkillLevel);
                    }

                    
                }
            }
        }

        private static int SkillPointsAtLevel(int characterLevel) {
            return (characterLevel - 1) / levelsPerPoint;
        }
    }
}
