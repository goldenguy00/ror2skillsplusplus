using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {
    public abstract class BaseSkillModifer : ISkillModifier {
        public SkillDef SkillDef { get; set; }
        public CharacterBody CharacterBody { get; set; }

        public abstract int MaxLevel { get; }

        internal BaseSkillModifer() { }
        public abstract IList<Type> GetEntityStateTypes();
        public virtual void OnSkillEnter(BaseState skillState, int level) { }
        public virtual void OnSkillExit(BaseState skillState, int level) { }
        public virtual void OnSkillLeveledUp(int level) { }
        public virtual string GetOverrideSkillDescriptionToken() {
            return null;
        }

        protected void ReportBroken(params String[] fields) {
            var fieldInfo = String.Join(",", fields);
            Logger.Warn("Skill {0} is broken. Cannot access the following items: {1}", SkillDef.name, fieldInfo);
        }

        #region Helpers
        public static float AdditiveScaling(float baseValue, float buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }
        public static int AdditiveScaling(int baseValue, int buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }
        public static float MultScaling(float baseValue, float multiplier, int level) {
            if(multiplier < 0) {
                return 1 / ((1 / baseValue) * (1 - multiplier * (level - 1)));
            } else {
                return baseValue * (1 + multiplier * (level - 1));
            }
        }
        public static int MultScaling(int baseValue, float multiplier, int level) {
            return (int)MultScaling((float)baseValue, multiplier, level);            
        }
        #endregion
    }
}
