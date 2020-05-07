using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace Skills.Modifiers {
    public abstract class BaseSkillModifer : ISkillModifier {
        public SkillDef SkillDef { get; set; }
        public CharacterBody CharacterBody { get; set; }

        public abstract int MaxLevel { get; }

        internal BaseSkillModifer() { }
        public abstract IList<Type> GetEntityStateTypes();
        public abstract void OnSkillEnter(BaseState skillState, int level);
        public abstract void OnSkillExit(BaseState skillState, int level);
        public abstract void OnSkillLeveledUp(int level);
        public virtual string GetOverrideSkillDescriptionToken() {
            return null;
        }

        #region Helpers
        protected static float AdditiveScaling(float baseValue, float buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }
        protected static int AdditiveScaling(int baseValue, int buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }
        protected static float MultScaling(float baseValue, float multiplier, int level) {
            return baseValue * (1 + multiplier * (level - 1));
        }
        protected static int MultScaling(int baseValue, float multiplier, int level) {
            return (int)(baseValue * (1 + multiplier * (level - 1)));
        }
        #endregion
    }
}
