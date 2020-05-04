using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2.Skills;

namespace Skills.Modifiers {
    class NoopSkillModifier : ISkillModifier {

        internal static NoopSkillModifier Instance = new NoopSkillModifier();

        public SkillDef SkillDef { get; set; }

        public int MaxLevel {
            get { return 1; }
        }

        public IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(BaseState) };
        }

        public void OnSkillLeveledUp(int level) {
            // do nothing
        }

        public void OnSkillWillBeUsed(BaseState skillState, int level) {
            // do nothing
        }

        public string GetOverrideSkillDescriptionToken() {
            return null;
        }
    }
}
