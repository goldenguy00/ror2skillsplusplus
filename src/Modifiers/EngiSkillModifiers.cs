using EntityStates.Engi;
using EntityStates.Engi.EngiWeapon;
using EntityStates.EngiTurret;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireGrenade")]
    class EngiGrenadesSkillModifier : TypedBaseSkillModifier<ChargeGrenades> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("PlaceMine")]
    class EngiMineSkillModifier : TypedBaseSkillModifier<FireMines> {
        public override int MaxLevel {
            get { return 2; }
        }
    }

    [SkillLevelModifier("PlaceSpiderMine")]
    class EngiSpiderMineSkillModifier : TypedBaseSkillModifier<FireSpiderMine> {
        public override int MaxLevel {
            get { return 2; }
        }
    }

    [SkillLevelModifier("PlaceBubbleShield")]
    class EngiBubbleShieldSkillModifier : TypedBaseSkillModifier<FireBubbleShield> {
        public override int MaxLevel {
            get { return 2; }
        }
    }

    [SkillLevelModifier("EngiHarpoons")]
    class EngiHarpoonsSkillModifier : TypedBaseSkillModifier<EntityStates.Engi.EngiMissilePainter.Paint> {
        public override int MaxLevel {
            get { return 2; }
        }
    }

    [SkillLevelModifier("PlaceTurret")]
    class EngiTurretSkillModifier : TypedBaseSkillModifier<PlaceTurret> {
        public override int MaxLevel {
            get { return 2; }
        }
    }

    [SkillLevelModifier("PlaceWalkerTurret")]
    class EngiWalkerTurretSkillModifier : TypedBaseSkillModifier<PlaceWalkerTurret> {
        public override int MaxLevel {
            get { return 2; }
        }
    }
}
