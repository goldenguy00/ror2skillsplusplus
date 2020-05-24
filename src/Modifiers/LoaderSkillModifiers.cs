using System;
using System.Collections.Generic;
using System.Text;

using SkillsPlusPlus.Modifiers;

using EntityStates.Loader;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("Knuckleboom")]
    class LoaderKnucklesSkillModifier : TypedBaseSkillModifier<SwingComboFist> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("FireHook")]
    class LoaderHookSkillModifier : TypedBaseSkillModifier<FireHook> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("FireYankHook")]
    class LoaderYankHookSkillModifier : TypedBaseSkillModifier<FireYankHook> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("ChargeFist")]
    class LoaderChargeFistSkillModifier : TypedBaseSkillModifier<ChargeFist> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("ChargeZapFist")]
    class LoaderChargeZapFistSkillModifier : TypedBaseSkillModifier<ChargeZapFist> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    // duplicate skill name
    [SkillLevelModifier("FireHook")]
    class LoaderPylonFistSkillModifier : TypedBaseSkillModifier<ThrowPylon> {

        public override int MaxLevel {
            get { return 1; }
        }

    }
}
