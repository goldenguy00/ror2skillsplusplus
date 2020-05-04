using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2.Skills;

namespace Skills.Modifiers {

    public abstract class TypedBaseSkillModifier<SkillState> : BaseSkillModifer where SkillState : BaseState {

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(SkillState) };
        }

        public sealed override void OnSkillWillBeUsed(BaseState skillState, int level) {
            if (skillState is SkillState) {
                this.OnSkillWillBeUsed(skillState as SkillState, level);
            } else {
                Logger.Warn("Unable to cast {0} to {1} for skill modifier {2}", skillState, typeof(SkillState).FullName, SkillDef.skillName);
            }
        }

        protected virtual void OnSkillWillBeUsed(SkillState skillState, int level) { 
            // no-op
        }
    }
}
