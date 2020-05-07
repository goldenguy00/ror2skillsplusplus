using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {
    class NoopSkillModifier : BaseSkillModifer {

        internal static NoopSkillModifier Instance = new NoopSkillModifier();

        public override int MaxLevel {
            get { return 1; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(BaseState) };
        }

        public override void OnSkillLeveledUp(int level) {
            // do nothing
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            // do nothing
        }

        public override void OnSkillExit(BaseState skillState, int level) {
            // do nothing
        }
    }
}
