using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe")]
    class TreebotSyringeSkillModifier : SimpleSkillModifier<FireSyringe> { }

    [SkillLevelModifier("AimMortarRain")]
    class TreebotMortarRainSkillModifier : SimpleSkillModifier<AimMortarRain> { }

    [SkillLevelModifier("AimMortar2")]
    class TreebotMortar2SkillModifier : SimpleSkillModifier<AimMortar2> { }

    [SkillLevelModifier("SonicBoom")]
    class TreebotSonicBoomSkillModifier : SimpleSkillModifier<ChargeSonicBoom> { }

    [SkillLevelModifier("SonicBoom2")]
    class TreebotPlantSonicBoomSkillModifier : SimpleSkillModifier<ChargePlantSonicBoom> { }

    [SkillLevelModifier("FireFlower2", "Chaotic Growth")]
    class TreebotFlowerSkillModifier : SimpleSkillModifier<PrepFlower2> { }
}
