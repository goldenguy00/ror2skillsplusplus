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
using R2API;
using GlobalEventManager = On.RoR2.GlobalEventManager;

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
    [SkillLevelModifier(new[] { "FireHandBeam", "ChargeHandBeam", "FireCorruptBeam", "CrushCorruption" }, typeof(FireHandBeam), typeof(ChargeHandBeam), typeof(FireCorruptHandBeam))]
    class VoidFiendHandBeamSkillModifier : BaseSkillModifier {
        GameObject surv;
        private int debuffTimerAdd;
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            surv = characterBody.gameObject;
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is FireHandBeam beam) {
                Logger.Debug("FireHandBeam");
                Logger.Debug(debuffTimerAdd);
                beam.damageCoefficient = MultScaling(beam.damageCoefficient, 0.10f, level);
                debuffTimerAdd = level;
                //beam.bulletCount = MultScaling(beam.bulletCount, 1, level);
            } else if (skillState is ChargeHandBeam chargebeam) {
                debuffTimerAdd = 0;
                Logger.Debug("ChargeHandBeam" + debuffTimerAdd);
                //chargebeam.muzzleflashEffectPrefab.transform.localScale = new Vector3(120f, 120f, 120f);
            } else if (skillState is FireCorruptHandBeam corruptbeam) {
                debuffTimerAdd = 0;
                Logger.Debug("FireCorruptHandBeam");
                Logger.Debug("FireCorruptHandBeam" + debuffTimerAdd);
                corruptbeam.beamVfxPrefab.transform.localScale = new Vector3(corruptbeam.beamVfxPrefab.transform.localScale.x, corruptbeam.beamVfxPrefab.transform.localScale.y, MultScaling(1, 0.30f, level)); //vfx should extend a bit farther imo 
                Logger.Debug("FireCorruptHandBeam" + corruptbeam.maxDistance);
                corruptbeam.maxDistance = MultScaling(corruptbeam.maxDistance, 0.25f, level);
                Logger.Debug("FireCorruptHandBeam" + corruptbeam.maxDistance);
                Logger.Debug("firecorruptmanged " + corruptbeam.damageCoefficientPerSecond);
                corruptbeam.damageCoefficientPerSecond = MultScaling(corruptbeam.damageCoefficientPerSecond, 0.15f, level);
                Logger.Debug("firecorruptmanged " + corruptbeam.damageCoefficientPerSecond);

                //add a buff here for movespeed 
            }
        }
        
        public override void SetupSkill()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManagerOnOnHitEnemy;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPIOnGetStatCoefficients;
            base.SetupSkill();
        }

        private void RecalculateStatsAPIOnGetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.gameObject == surv)
            {
                //check for buff here maybe hook onto where it applys slow debuff to player
            }
        }

        private void GlobalEventManagerOnOnHitEnemy(GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, DamageInfo damageinfo, GameObject victim)
        {
            orig(self, damageinfo, victim);
            if (damageinfo != null)
            {
                if (damageinfo.damageType.damageSource != DamageSource.Primary) return;
                if (damageinfo.attacker != surv) return;
                if(debuffTimerAdd == 0) return;
                Logger.Debug(debuffTimerAdd + " debuffing longer");
                victim.GetComponent<CharacterBody>().AddTimedBuff(RoR2Content.Buffs.Slow50, 3 + debuffTimerAdd); // 3 is the one the damagetype adds 
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
