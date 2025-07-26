using System;
using RoR2;
using UnityEngine;

namespace SkillsPlusPlus.Util
{
    internal static class SkillLocatorExtension
    {

        public static GenericSkill FindGenericSkillBySkillDef(this SkillLocator skillLocator, string skillName, bool allowMatchBaseSkill = false)
        {
            GenericSkill[] genericSkills = skillLocator.FindAllGenericSkills();
            return Array.Find(genericSkills, genericSkill =>
            {
                return (allowMatchBaseSkill && genericSkill.baseSkill.skillName == skillName) || ((ScriptableObject)genericSkill.skillDef)?.name == skillName;
            });
        }

        public static GenericSkill[] FindAllGenericSkills(this SkillLocator skillLocator)
        {
            return skillLocator.GetComponents<GenericSkill>();
        }

    }
}
