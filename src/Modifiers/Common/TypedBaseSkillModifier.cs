using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {

    public abstract class TypedBaseSkillModifier<SkillState> : BaseSkillModifier where SkillState : BaseState {

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(SkillState) };
        }

        public sealed override void OnSkillEnter(BaseState skillState, int level) {
            if (skillState is SkillState) {
                this.OnSkillEnter(skillState as SkillState, level);
            } else {
                Logger.Warn("Unable to cast {0} to {1}", skillState, typeof(SkillState).FullName);
            }
        }

        public sealed override void OnSkillExit(BaseState skillState, int level) {
            if (skillState is SkillState) {
                this.OnSkillExit(skillState as SkillState, level);
            } else {
                Logger.Warn("Unable to cast {0} to {1}", skillState, typeof(SkillState).FullName);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            // no-op
        }

        public virtual void OnSkillEnter(SkillState skillState, int level) {
            // no-op
        }

        public virtual void OnSkillExit(SkillState skillState, int level) {
            // no-op
        }
    }
}
