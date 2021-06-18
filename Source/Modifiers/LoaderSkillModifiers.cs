using System;
using System.Collections.Generic;
using System.Text;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;

using SkillsPlusPlus.Modifiers;

using EntityStates;
using EntityStates.Loader;

using UnityEngine;
using RoR2.UI;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("Knuckleboom", typeof(SwingComboFist))]
    class LoaderKnucklesSkillModifier : SimpleSkillModifier<SwingComboFist> {

        public override void OnSkillEnter(SwingComboFist skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.20f, level);
            SwingComboFist.barrierPercentagePerHit = AdditiveScaling(0.05f, 0.01f, level);
        }

        internal static void PatchSkillName() {
            var loaderBody = Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
            if(loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach(SkillFamily.Variant variant in skillLocator.primary.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if(skillDef != null) {
                        if(skillDef.skillNameToken == "LOADER_PRIMARY_NAME") {
                            skillDef.skillName = "Knuckleboom";
                        }
                    }
                }
            }
        }
    }

    [SkillLevelModifier("ThunderSlam", typeof(GroundSlam), typeof(PreGroundSlam))]
    class LoaderThunderSlamSkillModifier : BaseSkillModifier {

        static float baseUpwardVelocity = 0f;
        static float baseVerticalAcceleration = 0f;
        static float baseBlastRadius = 0f;
        static float maxVerticalCap = 0f;
        static Vector3 initialPosition;

        public override void OnSkillEnter(BaseState skillState, int level) {

            if(skillState is GroundSlam)
            {
                initialPosition = skillState.outer.commonComponents.characterBody.transform.position;

                GroundSlam.blastDamageCoefficient = MultScaling(20f, 0.5f, level);
            }

            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(Mathf.Abs(baseUpwardVelocity) < 0.01f)
            {
                baseUpwardVelocity = PreGroundSlam.upwardVelocity;
                baseVerticalAcceleration = GroundSlam.verticalAcceleration;
                baseBlastRadius = GroundSlam.blastRadius;

                FindSkillUpgrade(characterBody, "ThrowPylon");
            }

            PreGroundSlam.upwardVelocity = AdditiveScaling(baseUpwardVelocity, baseUpwardVelocity * 0.25f, level);
            GroundSlam.verticalAcceleration = AdditiveScaling(baseVerticalAcceleration, baseVerticalAcceleration * 0.25f, level);
            maxVerticalCap = AdditiveScaling(0f, 10f, level);
        }

        public static void GroundSlamFixedUpdate(On.EntityStates.Loader.GroundSlam.orig_FixedUpdate orig, EntityStates.Loader.GroundSlam self)
        {
            if (self.detonateNextFrame && registeredSkill && registeredSkill.skillLevel > 0)
            {
                float distanceTravelled = Vector3.Distance(self.outer.commonComponents.characterBody.transform.position, initialPosition);
                float distanceRatio = Mathf.Clamp(distanceTravelled / maxVerticalCap, 0f, 1f);

                GroundSlam.blastRadius = baseBlastRadius * (1 + Mathf.Lerp(0.1f, 1f, distanceRatio) * registeredSkill.skillLevel);
            }
            orig(self);
        }

        internal static void PatchSkillName() {
            var loaderBody = Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
            if(loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach(SkillFamily.Variant variant in skillLocator.special.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if(skillDef != null) {
                        if(skillDef.skillNameToken == "LOADER_SPECIAL_ALT_NAME") {
                            skillDef.skillName = "ThunderSlam";
                        }
                    }
                }
            }
        }
    }

    [SkillLevelModifier("FireHook", typeof(FireHook))]
    class LoaderHookSkillModifier : SimpleSkillModifier<FireHook> {

        public override void OnSkillEnter(FireHook skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileGrappleController grappleController)) {
                grappleController.maxTravelDistance = MultScaling(80, 0.30f, level);
            }
            CharacterBody body = skillState.outer.commonComponents.characterBody;
            GenericSkill utilitySkill = skillState.outer?.commonComponents.skillLocator?.utility;
            if(utilitySkill != null && utilitySkill.stock <= utilitySkill.maxStock) {
                // utilitySkill.AddOneStock();
            }
        }

        public override void OnSkillExit(FireHook skillState, int level) {
            base.OnSkillExit(skillState, level);

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(characterBody.crosshairPrefab.TryGetComponent(out LoaderHookCrosshairController hookCrosshairController)) {
                hookCrosshairController.range = MultScaling(80, 0.30f, level);
            }
        }

    }

    [SkillLevelModifier("FireYankHook", typeof(FireYankHook))]
    class LoaderYankHookSkillModifier : SimpleSkillModifier<FireYankHook> {

        public override void OnSkillEnter(FireYankHook skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileGrappleController grappleController)) {
                grappleController.maxTravelDistance = MultScaling(80, 0.15f, level);
            }
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(characterBody.crosshairPrefab.TryGetComponent(out LoaderHookCrosshairController hookCrosshairController)) {
                hookCrosshairController.range = MultScaling(80, 0.15f, level);
            }
            FireYankHook.damageCoefficient = MultScaling(3.2f, 0.20f, level);
        }

    }

    [SkillLevelModifier(new string[] { "ChargeFist", "Megaton Punch" }, typeof(ChargeFist), typeof(SwingChargedFist))]
    class LoaderChargeFistSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState baseState, int level) {
            base.OnSkillEnter(baseState, level);
            if(baseState is ChargeFist) {
                ChargeFist chargeState = (ChargeFist)baseState;
                chargeState.baseChargeDuration = MultScaling(chargeState.baseChargeDuration, 0.10f, level); // +10% max charge duration
            } else if(baseState is SwingChargedFist) {
                SwingChargedFist swingState = (SwingChargedFist)baseState;
                swingState.maxLungeSpeed = MultScaling(swingState.maxLungeSpeed, 0.15f, level); // +10% max lunge speed
                swingState.maxPunchForce = MultScaling(swingState.maxPunchForce, 0.15f, level); // +10% max pucnh force
                // swingState.maxDuration = MultScaling(swingState.maxDuration, 0.10f, level); // +10% max launge speed
            }
            
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            BaseSwingChargedFist.velocityDamageCoefficient = MultScaling(0.3f, 0.20f, level);
        }

    }

    [SkillLevelModifier(new string[] { "ChargeZapFist", "Thundercrash" }, typeof(ChargeZapFist), typeof(SwingZapFist))]
    class LoaderChargeZapFistSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState baseState, int level) {
            base.OnSkillEnter(baseState, level);
            if(baseState is ChargeZapFist) {
                ChargeZapFist chargeState = (ChargeZapFist)baseState;
            } else if(baseState is SwingZapFist) {
                SwingZapFist swingState = (SwingZapFist)baseState;
                swingState.damageCoefficient = MultScaling(swingState.damageCoefficient, 0.15f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            SwingZapFist.selfKnockback = MultScaling(7000, 0.015f, level); // +10% knockback
            if(SwingZapFist.overchargeImpactEffectPrefab.TryGetComponent(out ProjectileProximityBeamController proximityBeamController)) {
                proximityBeamController.attackRange = MultScaling(40, 0.20f, level);
            }
        }

    }

    // duplicate skill name
    [SkillLevelModifier("ThrowPylon", typeof(ThrowPylon))]
    class LoaderThrowPylonSkillModifier : SimpleSkillModifier<ThrowPylon> {

        internal static void PatchSkillName() {
            var loaderBody = Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
            if(loaderBody.TryGetComponent(out SkillLocator skillLocator)) {
                foreach(SkillFamily.Variant variant in skillLocator.special.skillFamily.variants) {
                    SkillDef skillDef = variant.skillDef;
                    if(skillDef != null) {
                        if(skillDef.skillNameToken == "LOADER_SPECIAL_NAME") {
                            skillDef.skillName = "ThrowPylon";
                        }
                    }
                }
            }
        }

        public override void OnSkillEnter(ThrowPylon skillState, int level) {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", ThrowPylon.baseDuration, ThrowPylon.damageCoefficient);
            ThrowPylon.damageCoefficient = MultScaling(1.0f, 0.2f, level);
            if(ThrowPylon.projectilePrefab.TryGetComponent(out ProjectileProximityBeamController proximityBeamController)) {
                //proximityBeamController.bounces = (int)AdditiveScaling(1, 0.5f, level);
                proximityBeamController.attackRange = MultScaling(15f, 0.20f, level);
                //proximityBeamController.attackFireCount = AdditiveScaling(6, 1, level);
                proximityBeamController.attackInterval = MultScaling(1.0f, -0.20f, level);
            }
        }

    }
}
