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

    [SkillLevelModifier("FireBlaster", typeof(FireHandBeam), typeof(ChargeHandBeam))]
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

    [SkillLevelModifier("MegaBlaster", typeof(ChargeMegaBlaster), typeof(FireMegaBlasterBase),
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

        internal static void PatchSkillName() {
            var loaderBody = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            if (loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach (SkillFamily.Variant variant in skillLocator.secondary.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if (skillDef != null) {
                        if (skillDef.skillNameToken == "VOIDSURVIVOR_SECONDARY_NAME") {
                            skillDef.skillName = "MegaBlaster";
                        }
                    }
                }
            }
        }

        public override void SetupSkill() {
            PatchSkillName();

            base.SetupSkill();
        }
    }

    [SkillLevelModifier("VoidBlinkUp", typeof(VoidBlinkUp))]
    class VoidFiendVoidBlinkUpSkillModifier : SimpleSkillModifier<VoidBlinkUp> {

        public override void OnSkillEnter(VoidBlinkUp skillState, int level) {
            Logger.Debug("VoidBlinkUp");
        }

        internal static void PatchSkillName() {
            var loaderBody = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            if (loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach (SkillFamily.Variant variant in skillLocator.utility.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if (skillDef != null) {
                        if (skillDef.skillNameToken == "VOIDSURVIVOR_UTILITY_NAME") {
                            skillDef.skillName = "VoidBlinkUp";
                        }
                    }
                }
            }
        }

        public override void SetupSkill() {
            PatchSkillName();

            base.SetupSkill();
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

        internal static void PatchSkillName() {
            var loaderBody = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            if (loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach (SkillFamily.Variant variant in skillLocator.special.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if (skillDef != null) {
                        if (skillDef.skillNameToken == "VOIDSURVIVOR_SPECIAL_NAME") {
                            skillDef.skillName = "CrushCorruption";
                        }
                    }
                }
            }
        }

        public override void SetupSkill() {
            PatchSkillName();

            base.SetupSkill();
        }
    }
}
