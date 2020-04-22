using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;

namespace Skills {
    class SkillPointsController : MonoBehaviour {

#if DEBUG
        private static int levelsPerPoint = 1;
        private static int skillLevelScaling = 2;
#else
        private static int levelsPerPoint = 5;
#endif

        private CharacterBody body;
        private SkillLevelIconController[] skillIconControllers;

       //  private Dictionary<SkillSlot, int> spentSkillPoints;
        private Dictionary<SkillSlot, int> skillLevels;

        private int earnedSkillPoints = 0;
        private int unspentSkillPoints = 0;

        void Awake() {
            // this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillLevels = new Dictionary<SkillSlot, int>();
            foreach(SkillSlot value in Enum.GetValues(typeof(SkillSlot))) {
                skillLevels[value] = 1;
            }
        }

        public void SetCharacterBody(CharacterBody body) {
            this.body = body;
        }

        public void SetSkillIconControllers(SkillLevelIconController[] skillIconControllers) {
            this.skillIconControllers = skillIconControllers;
            foreach(SkillLevelIconController skillIconController in skillIconControllers) {
                skillIconController.OnBuy += this.OnBuySkill;
            }
        }

        void Update() {
            if(body == null)
                return;

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

            Debug.LogFormat("TestCallStack: {0}",
                          Environment.StackTrace);

            if (skillLevels.TryGetValue(skillSlot, out int skillLevel)) {
                SkillDef skillDef = body.skillLocator.GetSkill(skillSlot).skillDef;
                BaseSkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);

                if (skillLevel >= modifier.MaxLevel()) {
                    return;
                }

                unspentSkillPoints--;


                // increment and store the new skill level
                skillLevels[skillSlot] = ++skillLevel;
                Debug.LogFormat("SkillSlot {0} @ level {1}", Enum.GetName(typeof(SkillSlot), skillSlot), skillLevel);

                // find an notify the modifer to update the skill's parameters
                if (modifier != null)
                {
                    modifier.ApplyChanges(skillDef, skillLevel + 1);
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
                        BaseSkillModifier modifier = SkillModifierManager.GetSkillModifier(body.skillLocator.GetSkill(slot).skillDef);
                    
                        int requiredLevelToBuySkill = (currentSkillLevel / skillLevelScaling);
                        // has skillpoints to spend
                        // meets required character level
                        // and the skill is less than its max level
                        skillLevelIconController.SetCanUpgrade(unspentSkillPoints > 0 && characterLevel >= requiredLevelToBuySkill && currentSkillLevel < modifier.MaxLevel());
                        skillLevelIconController.SetLevel(currentSkillLevel);
                    }

                    
                }
            }}

        private static int SkillPointsAtLevel(int characterLevel) {
            return (characterLevel - 1) / levelsPerPoint;
        }
    }
}
