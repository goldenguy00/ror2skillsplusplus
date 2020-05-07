using System;

namespace SkillsPlusPlus {

    [AttributeUsage(AttributeTargets.Class)]
    public class SkillLevelModifierAttribute : Attribute {

        public readonly string skillName;

        public SkillLevelModifierAttribute(string skillName) {
            this.skillName = skillName;
        }

    }
}
