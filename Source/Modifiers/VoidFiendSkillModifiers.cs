using System;
using System.Collections.Generic;
using UnityEngine;

using SkillsPlusPlus.Modifiers;
using RoR2;

using EntityStates;
using EntityStates.VoidSurvivor.Weapon;
using static EntityStates.VoidSurvivor.VoidBlinkBase;

using RoR2.Projectile;
using RoR2.Skills;

using System.Linq;
using UnityEngine.AddressableAssets;
using EntityStates.VoidSurvivor.CorruptMode;

namespace SkillsPlusPlus.Source.Modifiers {
    class VoidFiendSkillModifiers {
    }


    //[SkillLevelModifier("PlaceSpiderMine", typeof(ChargeCrabCannon), typeof(ChargeCorruptHandBeam), typeof(ChargeCrushBase), typeof(ChargeCrushHealth), typeof(CrushBase), typeof(CrushHealth), typeof(EnterSwingMelee), typeof(FireBlaster1), typeof(FireBlaster2), typeof(FireBlaster3), typeof(FireBlaster4),
    //    typeof(FireBlasterBase), typeof(FireCorruptDisks), typeof(FireCorruptHandBeam), typeof(FireCrabCannon), typeof(FireRepulsion), typeof(FireTwinBlaster), typeof(ReadyMegaBlaster), typeof(Suppress)
    //    , typeof(SwingMelee1), typeof(SwingMelee2), typeof(SwingMelee3), typeof(SwingMeleeBase))]
    //class VoidFiendSkillModifier : BaseSkillModifier {

    //    public override void OnSkillEnter(BaseState skillState, int level) {
    //        Logger.Debug("Skill entered: " + skillState.ToString());
    //    }

    //}

    [SkillLevelModifier("FireHandBeam", typeof(FireHandBeam), typeof(ChargeHandBeam))]
    class VoidFiendHandBeamSkillModifier : BaseSkillModifier {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);

            if(skillState is FireHandBeam) {
                Logger.Debug("FireHandBeam");
            } else if (skillState is ChargeHandBeam) {
                Logger.Debug("ChargeHandBeam");
            }
        }
    }

    [SkillLevelModifier("FireCorruptBeam", typeof(FireCorruptHandBeam), typeof(ChargeCorruptHandBeam))]
    class VoidFiendCorruptHandBeamSkillModifier : BaseSkillModifier {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);

            if (skillState is FireCorruptHandBeam) {
                Logger.Debug("FireCorruptHandBeam");
            } else if (skillState is ChargeCorruptHandBeam) {
                Logger.Debug("ChargeCorruptHandBeam");
            }
        }
    }

    [SkillLevelModifier("ChargeMegaBlaster", typeof(ChargeMegaBlaster), typeof(FireMegaBlasterBase),
        typeof(FireMegaBlasterBig), typeof(FireMegaBlasterSmall))]
    class VoidFiendChargeMegaSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState skillState, int level) {
            if (skillState is ChargeMegaBlaster) {
                Logger.Debug("ChargeMegaBlaster");
            } else if (skillState is FireMegaBlasterBase) {
                Logger.Debug("FireMegaBlasterBase");
            } else if (skillState is FireMegaBlasterBig) {
                Logger.Debug("FireMegaBlasterBig");
            } else if (skillState is FireMegaBlasterSmall) {
                Logger.Debug("FireMegaBlasterSmall");
            }
        }
    }

    [SkillLevelModifier("FireCorruptDisk", typeof(FireCorruptDisks))]
    class VoidFiendFireCorruptDiskSkillModifier : SimpleSkillModifier<FireCorruptDisks> {

        public override void OnSkillEnter(FireCorruptDisks skillState, int level) {
            Logger.Debug("FireCorruptDisks");
        }
    }

    [SkillLevelModifier("VoidBlinkUp", typeof(VoidBlinkUp))]
    class VoidFiendVoidBlinkUpSkillModifier : SimpleSkillModifier<VoidBlinkUp> {

        public override void OnSkillEnter(VoidBlinkUp skillState, int level) {
            Logger.Debug("VoidBlinkUp");
        }
    }

    [SkillLevelModifier("VoidBlinkDown", typeof(VoidBlinkDown))]
    class VoidFiendVoidBlinkDownSkillModifier : SimpleSkillModifier<VoidBlinkDown> {

        public override void OnSkillEnter(VoidBlinkDown skillState, int level) {
            Logger.Debug("VoidBlinkDown");
        }
    }

    [SkillLevelModifier("CrushCorruption", typeof(CrushCorruption), typeof(ChargeCrushCorruption))]
    class VoidFiendCrushCorruptionSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState skillState, int level) {
            if (skillState is CrushCorruption) {
                Logger.Debug("CrushCorruption");
            } else if (skillState is ChargeCrushCorruption) {
                Logger.Debug("ChargeCrushCorruption");
            }
        }
    }

    [SkillLevelModifier("CrushHealth", typeof(CrushHealth), typeof(ChargeCrushHealth))]
    class VoidFiendCrushHealthSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState skillState, int level) {
            if (skillState is CrushHealth) {
                Logger.Debug("CrushHealth");
            } else if (skillState is ChargeCrushHealth) {
                Logger.Debug("ChargeCrushHealth");
            }
        }
    }
}
