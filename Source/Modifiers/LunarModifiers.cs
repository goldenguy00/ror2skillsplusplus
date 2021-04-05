using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
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

        [SkillLevelModifier("LunarUtilityReplacement", typeof(GhostUtilitySkillState))]
        class StridesOfHeresySkillModifier : SimpleSkillModifier<GhostUtilitySkillState> {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);
                GhostUtilitySkillState.moveSpeedCoefficient = MultScaling(1.3f, 0.1f, level); // +10% 
                GhostUtilitySkillState.healFrequency = MultScaling(5, 0.15f, level); // +15%
            }

        }

    }
}