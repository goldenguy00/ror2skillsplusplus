using System;
using RoR2.Skills;
using EntityStates;
using UnityEngine;

namespace Skills {

    [AttributeUsage(AttributeTargets.Class)]
    class SkillLevelModifierAttribute : Attribute {

        public readonly string skillName;

        public SkillLevelModifierAttribute(string skillName) {
            this.skillName = skillName;
        }

    }

    internal interface ISkillModifier {

        Type GetStateType();

        int MaxLevel { get; }
        SkillDef SkillDef { get; set; }
        void OnSkillWillBeUsed(BaseState skillState, int level);
        void OnSkillLeveledUp(int level);

    }

    public abstract class BaseSkillModifier<SkillState> : ISkillModifier where SkillState : BaseState {
        public SkillDef SkillDef { get; set; }

        public abstract int MaxLevel { get; }

        internal BaseSkillModifier() { }

        public Type GetStateType() {
            return typeof(SkillState);
        }

        public void OnSkillWillBeUsed(BaseState skillState, int level) {
            if (skillState is SkillState) {
                this.OnSkillWillBeUsed(skillState as SkillState, level);
            } else {
                Debug.LogFormat("Unable to cast {0} to {1} for skill modifier {2}", skillState, typeof(SkillState).FullName, SkillDef.skillName);
            }
        }

        protected virtual void OnSkillWillBeUsed(SkillState skillState, int level) { 
            // do nothing in base implementation
        }

        public abstract void OnSkillLeveledUp(int level);

        protected static float AdditiveScaling(float baseValue, float buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }
        protected static int AdditiveScaling(int baseValue, int buffValue, int level) {
            return baseValue + buffValue * (level - 1);
        }

        protected static float LogScaling(float baseValue, float buffValue, int level) {
            return baseValue + (Mathf.Log(level) * buffValue);
        }
    }
}
