using System;
using System.Collections.Generic;
using System.Text;

using EntityStates.GlobalSkills.LunarNeedle;
using EntityStates;

using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {
    class LunarModifiers {

        [SkillLevelModifier("LunarPrimaryReplacement", typeof(FireLunarNeedle))]
        class VisionsOfHeresySkillModifier : SimpleSkillModifier<FireLunarNeedle> {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);
                skillDef.baseMaxStock = AdditiveScaling(12, 2, level);
                FireLunarNeedle.damageCoefficient = MultScaling(0.05f, 0.2f, level);
            }

        }

        //class StridesOfHeresySkillModifier : SimpleSkillModifier<GhostUtilitySkillState> {

        //    public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
        //        base.OnSkillLeveledUp(level, characterBody, skillDef);
        //        skillDef.baseMaxStock = AdditiveScaling(12, 2, level);
        //        FireLunarNeedle.damageCoefficient = MultScaling(1.2f, 0.2f, level);
        //    }

        //}

    }
}
