using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe", typeof(FireSyringe))]
    class TreebotSyringeSkillModifier : SimpleSkillModifier<FireSyringe> { }

    [SkillLevelModifier("AimMortarRain", typeof(AimMortarRain))]
    class TreebotMortarRainSkillModifier : SimpleSkillModifier<AimMortarRain> { }

    [SkillLevelModifier("AimMortar2", typeof(AimMortar2))]
    class TreebotMortar2SkillModifier : SimpleSkillModifier<AimMortar2> { }

    [SkillLevelModifier("SonicBoom", typeof(ChargeSonicBoom))]
    class TreebotSonicBoomSkillModifier : SimpleSkillModifier<ChargeSonicBoom> { }

    [SkillLevelModifier("SonicBoom2", typeof(ChargePlantSonicBoom))]
    class TreebotPlantSonicBoomSkillModifier : SimpleSkillModifier<ChargePlantSonicBoom> { }

    [SkillLevelModifier(new string[] { "FireFlower2", "Chaotic Growth" }, typeof(PrepFlower2))]
    class TreebotFlowerSkillModifier : SimpleSkillModifier<PrepFlower2> { }
}
