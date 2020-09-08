using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Rewired;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Util {
    static class SkillLocatorExtension {

        public static GenericSkill FindGenericSkillBySkillDef(this SkillLocator skillLocator, string skillName, bool allowMatchBaseSkill = false) {
            GenericSkill[] genericSkills = skillLocator.FindAllGenericSkills();
            return Array.Find(genericSkills, genericSkill => {
                return (allowMatchBaseSkill && genericSkill.baseSkill.skillName == skillName) || genericSkill.skillDef.skillName == skillName;
            });
        }

        public static GenericSkill[] FindAllGenericSkills(this SkillLocator skillLocator) {
            return skillLocator.GetComponents<GenericSkill>();
        }

    }
}
