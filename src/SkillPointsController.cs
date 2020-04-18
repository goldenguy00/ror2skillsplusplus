using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
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

        private Dictionary<SkillSlot, int> spentSkillPoints;
        private Dictionary<SkillSlot, int> skillLevels;

        private int earnedSkillPoints = 0;
        private int unspentSkillPoints = 0;

        void Awake() {
            this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillLevels = new Dictionary<SkillSlot, int>();
        }

        public void SetCharacterBody(CharacterBody body) {
            this.body = body;
        }

        public void SetSkillIconControllers(SkillLevelIconController[] skillIconControllers) {
            this.skillIconControllers = skillIconControllers;
        }

        void Update() {
            if(body == null)
                return;

#if DEBUG
            if(Input.GetKey(KeyCode.Equals)) {
                TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong) (500 * Time.deltaTime));
            }
#endif
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

            if(skillIconControllers != null) {
                foreach(SkillLevelIconController skillLevelIconController in skillIconControllers) {
                    SkillSlot slot = skillLevelIconController.SkillSlot;
                    spentSkillPoints.TryGetValue(slot, out int spentOnSlot);
                    skillLevels.TryGetValue(slot, out int currentSkillLevel);

                    skillLevelIconController.SetCanUpgrade(unspentSkillPoints > 0 && spentOnSlot <= ((currentSkillLevel + 1) / skillLevelScaling));
                }
            }

        }

        private static int SkillPointsAtLevel(int characterLevel) {
            return (characterLevel - 1) / levelsPerPoint;
        }
    }
}
