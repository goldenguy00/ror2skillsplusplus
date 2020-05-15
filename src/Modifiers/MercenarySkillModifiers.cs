using System;
using System.Collections.Generic;
using System.Text;

using SkillsPlusPlus.Modifiers;

using EntityStates.Merc;
using EntityStates.Commando.CommandoWeapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("GroundLight")]
    class SwordSkillModifier : TypedBaseSkillModifier<GroundLight> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("Whirlwind")]
    class WhirlwindSkillModifier : TypedBaseSkillModifier<WhirlwindEntry> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("Uppercut")]
    class UppercutSkillModifier : TypedBaseSkillModifier<Uppercut> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("Dash1")]
    class AssaultSkillModifier : TypedBaseSkillModifier<Assaulter> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    // both Mercenary special skills have the same skill name
    [SkillLevelModifier("Evis")]
    class EviscerateSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 4; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() {
                typeof(EvisDash),
                typeof(ThrowEvisProjectile)
            };
        }
    }
}
