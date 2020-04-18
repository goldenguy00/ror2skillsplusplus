using System;
using System.Collections.Generic;
using System.Text;
using EntityStates.Huntress.HuntressWeapon;
using RoR2.Skills;

namespace Skills {

    [AttributeUsage(AttributeTargets.Class)]
    class SkillLevelModifierAttribute : Attribute {

        public readonly string skillName;

        public SkillLevelModifierAttribute(string skillName) {
            this.skillName = skillName;
        }

    }

    public abstract class BaseSkillModifier {

        internal BaseSkillModifier() { }

        public abstract int MaxLevel();

        public abstract void ApplyChanges(SkillDef skillDef, int level);

    }
}
