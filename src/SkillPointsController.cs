using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using EntityStates;
using R2API;
using R2API.Utils;
using Rewired;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;

namespace Skills {

    [RequireComponent(typeof(PlayerCharacterMasterController))]
    class SkillPointsController : MonoBehaviour {

#if DEBUG
        private static int levelsPerPoint = 1;
        private static int skillLevelScaling = 2;
#else
        private static int levelsPerPoint = 3;
        private static int skillLevelScaling = 1;
#endif
        private PlayerCharacterMasterController playerCharacterMasterController;
        private CharacterBody body { 
            get { return playerCharacterMasterController.master.GetBody(); }
        }
        private SkillLocator skillLocator {
            get { return body?.skillLocator; }
        }

        private TeamIndex PlayerTeamIndex {
            get { 
                if (playerCharacterMasterController.master.hasBody) {
                    return playerCharacterMasterController.master.GetBody().teamComponent.teamIndex;
                }
                return TeamIndex.None;
            }
        }

        private SkillLevelIconController[] skillIconControllers;

        //  private Dictionary<SkillSlot, int> spentSkillPoints;
        private Dictionary<SkillSlot, int> skillLevels;

        private int earnedSkillPoints = 0;
        private int unspentSkillPoints = 0;

        void Awake() {
            // this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillLevels = new Dictionary<SkillSlot, int>();
            foreach (SkillSlot value in Enum.GetValues(typeof(SkillSlot))) {
                skillLevels[value] = 1;
            }

            this.playerCharacterMasterController = this.GetComponent<PlayerCharacterMasterController>();

            if (this.playerCharacterMasterController.master.hasBody) {
                this.OnBodyStart(this.playerCharacterMasterController.master.GetBody());
            } else {
                this.playerCharacterMasterController.master.onBodyStart += this.OnBodyStart;
            }
            On.RoR2.EntityStateMachine.SetState += this.OnInterceptSetState;

            On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateState;
        }

        void OnDestroy() {
            this.playerCharacterMasterController.master.onBodyStart -= this.OnBodyStart;
            On.RoR2.EntityStateMachine.SetState -= this.OnInterceptSetState;
            On.RoR2.CharacterBody.RecalculateStats -= this.OnRecalculateState;
        }

        private void OnBodyStart(CharacterBody body) {
            //EnsureSkillModifiersAreInitialised(true);
        }
        private void OnRecalculateState(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self == this.body) {
                this.OnLevelChanged();
            }
        }

        private void OnInterceptSetState(On.RoR2.EntityStateMachine.orig_SetState orig, EntityStateMachine self, EntityState newState) {
            if (newState != null && self.networker != null && self.networker.localPlayerAuthority) {
                if (this.body != null && self.commonComponents.characterBody == this.body) {
                    if (newState is BaseState) {
                        ISkillModifier skillModifier = SkillModifierManager.GetSkillModifierForEntityStateType(newState.GetType());
                        if (skillModifier != null && skillModifier.SkillDef?.skillName != null) {
                            GenericSkill genericSkill = this.skillLocator.FindSkill(skillModifier.SkillDef.skillName);
                            if (genericSkill != null) {
                                SkillSlot skillSlot = this.skillLocator.FindSkillSlot(genericSkill);
                                if (skillSlot != null) {
                                    Logger.Debug("Successfully intercepted entity state {1} for skill named {0}", skillModifier.SkillDef.skillName, newState.GetType().Name);
                                    this.EnsureSkillModifiersAreInitialised();
                                    skillModifier.OnSkillWillBeUsed((BaseState)newState, this.skillLevels[skillSlot]);
                                } else {
                                    Logger.Error("Could not identify skill slot for generic skill {0}", genericSkill);
                                }
                            } else {
                                Logger.Error("Could not find generic skill instance for skill named {0}", skillModifier.SkillDef.skillName);
                            }
                        } else {

                        }
                    } else {
                        Logger.Debug(newState);
                    }
                } else {

                }
            }

            orig(self, newState);
        }

        private bool skillModifiersAreInitialised = false;
        private void EnsureSkillModifiersAreInitialised(bool force = false) {            
            if (skillModifiersAreInitialised && !force) {
                return;
            }

            var skillLocator = this.skillLocator;
            if (skillLocator == null) {
                Logger.Debug("Unable to initialise skill modifiers since there is no skill locator to use");
                return;
            }

            skillModifiersAreInitialised = true;
            foreach (SkillSlot skillSlot in Enum.GetValues(typeof(SkillSlot))) {
                GenericSkill genericSkill = skillLocator.GetSkill(skillSlot);
                if (genericSkill == null) {
                    Logger.Debug("Skipping slot {0} since there is no generic skill for it", skillSlot);
                    continue;
                }
                // clone the current skill definition since we are going to mutating it
                // and we don't want to persist any state between runs
                SkillDef skillDef = Instantiate(genericSkill.skillDef);
                ISkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);
                modifier.SkillDef = skillDef;
                modifier.OnSkillLeveledUp(skillLevels[skillSlot]);
                genericSkill.SetBaseSkill(skillDef);
            }
        }

        public void SetSkillIconControllers(SkillLevelIconController[] skillIconControllers) {
            if (this.skillIconControllers != null) {
                foreach (SkillLevelIconController skillIconController in this.skillIconControllers) {
                    skillIconController.OnUpgradeSkill -= this.OnBuySkill;
                }
            }
            this.skillIconControllers = skillIconControllers;
            if (skillIconControllers != null) {
                foreach (SkillLevelIconController skillIconController in skillIconControllers) {
                    skillIconController.OnUpgradeSkill += this.OnBuySkill;
                }
                RefreshIconControllers();
            }
        }

        void Update() {

            EnsureSkillModifiersAreInitialised();

            if (skillIconControllers != null) {
                bool infoButtonDown = playerCharacterMasterController?.networkUser?.inputPlayer?.GetButton(RewiredConsts.Action.Info) == true;
                foreach (SkillLevelIconController skillLevelIconController in skillIconControllers) {
                    skillLevelIconController.ShowBuyButton(infoButtonDown);
                }
            }



#if DEBUG
            if (Input.GetKeyDown(KeyCode.Equals) && this.PlayerTeamIndex != TeamIndex.None) {
                //TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(500 * Time.deltaTime));
                TeamManager.instance.SetTeamLevel(this.PlayerTeamIndex, TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex) + 1);
            }
#endif

        }

        private void OnBuySkill(SkillSlot skillSlot) {
            if (unspentSkillPoints <= 0) {
                return;
            }
            Logger.Debug("OnBuySkill({0})", Enum.GetName(typeof(SkillSlot), skillSlot));

            if (skillLevels.TryGetValue(skillSlot, out int skillLevel)) {
                SkillDef skillDef = skillLocator.GetSkill(skillSlot).skillDef;
                ISkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);

                if (skillLevel >= modifier.MaxLevel) {
                    return;
                }

                unspentSkillPoints--;


                // increment and store the new skill level
                skillLevels[skillSlot] = ++skillLevel;
                Logger.Debug("SkillSlot {0} @ level {1}", Enum.GetName(typeof(SkillSlot), skillSlot), skillLevel);

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
            int characterLevel = (int) TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex);
            Logger.Debug("OnLevelChanged({0}) for team {1}", characterLevel, PlayerTeamIndex);
            if (this.PlayerTeamIndex == TeamIndex.None) {
                return;
            }

            int newSkillPoints = Math.Max(0, SkillPointsAtLevel(characterLevel) - earnedSkillPoints);

            earnedSkillPoints += newSkillPoints;
            unspentSkillPoints += newSkillPoints;

            RefreshIconControllers();
        }

        private void RefreshIconControllers() {
            var skillLocator = this.skillLocator;
            if(skillIconControllers != null && this.PlayerTeamIndex != TeamIndex.None && skillLocator != null) {
                int characterLevel = (int) TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex);
                foreach (SkillLevelIconController skillLevelIconController in skillIconControllers) {

                    SkillSlot slot = skillLevelIconController.SkillSlot;
                    if (skillLevels.TryGetValue(slot, out int currentSkillLevel)) {
                        ISkillModifier modifier = SkillModifierManager.GetSkillModifier(skillLocator.GetSkill(slot).skillDef);
                        Logger.Debug("RefreshIconControllers - slot: {0}, skillLevelIconController: {1}, modifier: {2}", slot, skillLevelIconController, modifier);

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
