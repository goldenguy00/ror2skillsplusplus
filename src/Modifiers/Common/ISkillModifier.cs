using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {

    internal interface ISkillModifier {

        IList<Type> GetEntityStateTypes();

        int MaxLevel { get; }
        SkillDef SkillDef { get; set; }
        CharacterBody CharacterBody { get; set; }
        void OnSkillEnter(BaseState skillState, int level);
        void OnSkillExit(BaseState skillState, int level);
        void OnSkillLeveledUp(int level);

        string GetOverrideSkillDescriptionToken();

    }
}
