using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Rewired;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Util {
    static class SkillLocatorExtension {

        public static GenericSkill FindGenericSkill(this SkillLocator skillLocator, string skillName) {
            GenericSkill[] genericSkills = skillLocator.FindAllGenericSkills();
            return Array.Find(genericSkills, genericSkill => { return genericSkill.skillDef.skillName == skillName; });
        }

        public static GenericSkill[] FindAllGenericSkills(this SkillLocator skillLocator) {
            return skillLocator.GetComponents<GenericSkill>();
        }

    }
}
