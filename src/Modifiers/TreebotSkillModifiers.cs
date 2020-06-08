using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe")]
    class TreebotSyringeSkillModifier : TypedBaseSkillModifier<FireSyringe> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("AimMortarRain")]
    class TreebotMortarRainSkillModifier : TypedBaseSkillModifier<AimMortarRain> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("AimMortar2")]
    class TreebotMortar2SkillModifier : TypedBaseSkillModifier<AimMortar2> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("SonicBoom")]
    class TreebotSonicBoomSkillModifier : TypedBaseSkillModifier<ChargeSonicBoom> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("SonicBoom2")]
    class TreebotPlantSonicBoomSkillModifier : TypedBaseSkillModifier<ChargePlantSonicBoom> {

        public override int MaxLevel {
            get { return 1; }
        }
    }

    [SkillLevelModifier("FireFlower2", "Chaotic Growth")]
    class TreebotFlowerSkillModifier : TypedBaseSkillModifier<PrepFlower2> {

        public override int MaxLevel {
            get { return 1; }
        }
    }
}
