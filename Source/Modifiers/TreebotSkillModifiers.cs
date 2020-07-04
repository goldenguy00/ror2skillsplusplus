using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe")]
    class TreebotSyringeSkillModifier : SimpleSkillModifier<FireSyringe> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("AimMortarRain")]
    class TreebotMortarRainSkillModifier : SimpleSkillModifier<AimMortarRain> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("AimMortar2")]
    class TreebotMortar2SkillModifier : SimpleSkillModifier<AimMortar2> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("SonicBoom")]
    class TreebotSonicBoomSkillModifier : SimpleSkillModifier<ChargeSonicBoom> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("SonicBoom2")]
    class TreebotPlantSonicBoomSkillModifier : SimpleSkillModifier<ChargePlantSonicBoom> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("FireFlower2", "Chaotic Growth")]
    class TreebotFlowerSkillModifier : SimpleSkillModifier<PrepFlower2> {

        public override int MaxLevel {
            get { return 1; }
        }
    }
}
