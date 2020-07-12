using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {
    internal class NoopSkillModifier : BaseSkillModifier {

        internal static NoopSkillModifier Instance = new NoopSkillModifier();

        /// <inheritdoc/>
        public override int MaxLevel {
            get { return 1; }
        }

        /// <inheritdoc/>
        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(BaseState) };
        }

        /// <inheritdoc/>
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            // do nothing
        }

        /// <inheritdoc/>
        public override void OnSkillEnter(BaseState skillState, int level) {
            // do nothing
        }

        /// <inheritdoc/>
        public override void OnSkillExit(BaseState skillState, int level) {
            // do nothing
        }
    }
}
