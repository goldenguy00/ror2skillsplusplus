using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe")]
    class TreebotSyringeSkillModifier : TypedBaseSkillModifier<FireSyringe> { }

    [SkillLevelModifier("AimMortarRain")]
    class TreebotMortarRainSkillModifier : TypedBaseSkillModifier<AimMortarRain> { }

    [SkillLevelModifier("AimMortar2")]
    class TreebotMortar2SkillModifier : TypedBaseSkillModifier<AimMortar2> { }

    [SkillLevelModifier("SonicBoom")]
    class TreebotSonicBoomSkillModifier : TypedBaseSkillModifier<ChargeSonicBoom> { }

    [SkillLevelModifier("SonicBoom2")]
    class TreebotPlantSonicBoomSkillModifier : TypedBaseSkillModifier<ChargePlantSonicBoom> { }

    [SkillLevelModifier("FireFlower2", "Chaotic Growth")]
    class TreebotFlowerSkillModifier : TypedBaseSkillModifier<PrepFlower2> { }
}
