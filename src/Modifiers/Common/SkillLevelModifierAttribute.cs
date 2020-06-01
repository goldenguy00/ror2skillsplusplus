using System;
using System.Collections.Generic;
using System.Linq;

namespace SkillsPlusPlus.Modifiers {

    [AttributeUsage(AttributeTargets.Class)]
    public class SkillLevelModifierAttribute : Attribute {

        public readonly List<string> skillNames = new List<string>();

        public SkillLevelModifierAttribute(string firstSkillName, params string[] skillNames) {
            this.skillNames.Add(firstSkillName);
            this.skillNames.AddRange(skillNames);
        }

    }
}
