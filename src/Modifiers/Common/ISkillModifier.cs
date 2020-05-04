using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2.Skills;

namespace Skills.Modifiers {

    internal interface ISkillModifier {

        IList<Type> GetEntityStateTypes();

        int MaxLevel { get; }
        SkillDef SkillDef { get; set; }
        void OnSkillWillBeUsed(BaseState skillState, int level);
        void OnSkillLeveledUp(int level);

        string GetOverrideSkillDescriptionToken();

    }
}
