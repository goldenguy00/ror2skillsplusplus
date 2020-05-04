using System;
using System.Collections.Generic;
using RoR2.Skills;
using EntityStates;
using UnityEngine;

namespace Skills {

    [AttributeUsage(AttributeTargets.Class)]
    public class SkillLevelModifierAttribute : Attribute {

        public readonly string skillName;

        public SkillLevelModifierAttribute(string skillName) {
            this.skillName = skillName;
        }

    }
}
