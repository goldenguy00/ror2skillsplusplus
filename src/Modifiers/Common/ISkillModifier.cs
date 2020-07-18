using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {

    internal interface ISkillModifier {

        IList<Type> GetEntityStateTypes();

        string skillName { get; set; }
        void OnSkillEnter(BaseState skillState, int level);
        void OnSkillExit(BaseState skillState, int level);
        void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef);

        string GetOverrideSkillDescriptionToken();

    }
}
