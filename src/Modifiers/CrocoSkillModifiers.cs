using System;
using System.Collections.Generic;
using System.Text;

using SkillsPlusPlus.Modifiers;

using EntityStates.Croco;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("Poison")]
    class CrocoPoisonSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 1; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>();
        }

    }

    [SkillLevelModifier("Blight")]
    class CrocoBlightSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 1; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>();
        }

    }

    [SkillLevelModifier("CrocoSlash")]
    class CrocoSlashSkillModifier : TypedBaseSkillModifier<Slash> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("CrocoSpit")]
    class CrocoSpitSkillModifier : TypedBaseSkillModifier<FireSpit> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("CrocoBite")]
    class CrocoBiteSkillModifier : TypedBaseSkillModifier<Bite> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("CrocoLeap")]
    class CrocoLeapSkillModifier : TypedBaseSkillModifier<Leap> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("CrocoChainableLeap")]
    class CrocoChainableLeapSkillModifier : TypedBaseSkillModifier<ChainableLeap> {

        public override int MaxLevel {
            get { return 1; }
        }

    }

    [SkillLevelModifier("CrocoDisease")]
    class CrocoDiseaseSkillModifier : TypedBaseSkillModifier<Disease> {

        public override int MaxLevel {
            get { return 1; }
        }

    }
}
