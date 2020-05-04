using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using R2API.AssetPlus;
using UnityEngine;
using RoR2.UI;
using RoR2;
using System.Collections.Generic;
using System;
using EntityStates;

namespace Skills.Modifiers {

    [SkillLevelModifier("FireSeekingArrow")]
    class HuntressSeekingArrowSkillModifier : TypedBaseSkillModifier<FireSeekingArrow> {
        public override int MaxLevel {
            get { return 4; }
        }
        protected override void OnSkillEnter(FireSeekingArrow skillState, int level) {
            base.OnSkillEnter(skillState, level);
            var huntressTracker = skillState.outer.GetComponent<HuntressTracker>();
            Logger.Debug("orbProcCoefficient: {0}, trackingDistance: {1}, trackingAngle: {3}, maxArrowCount: {2}", skillState.orbProcCoefficient, huntressTracker.maxTrackingDistance, skillState.maxArrowCount, huntressTracker.maxTrackingAngle);
            huntressTracker.maxTrackingDistance = AdditiveScaling(60, 20, level); // 33%
            huntressTracker.maxTrackingAngle = AdditiveScaling(30, 5, level); // 16%
            skillState.orbProcCoefficient = AdditiveScaling(1f, 0.2f, level);
            //skillState.orbProcCoefficient;
        }

        public override void OnSkillLeveledUp(int level) {
            
        }
    }

    [SkillLevelModifier("FireFlurrySeekingArrow")]
    class HuntressFlurrySkillModifier : TypedBaseSkillModifier<FireFlurrySeekingArrow> {

        public override int MaxLevel {
            get { return 4; }
        }

        protected override void OnSkillEnter(FireFlurrySeekingArrow skillState, int level) {
            base.OnSkillEnter(skillState, level);
            var huntressTracker = skillState.outer.GetComponent<HuntressTracker>();
            Logger.Debug("orbProcCoefficient: {0}, trackingDistance: {1}, trackingAngle: {3}, maxArrowCount: {2}", skillState.orbProcCoefficient, huntressTracker.maxTrackingDistance, skillState.maxArrowCount, huntressTracker.maxTrackingAngle);
            huntressTracker.maxTrackingDistance = AdditiveScaling(60, 10, level);
            huntressTracker.maxTrackingAngle = AdditiveScaling(30, 5, level); // 16%
            skillState.maxArrowCount = AdditiveScaling(3, 1, level);
            //skillState.orbProcCoefficient;
        }

        public override void OnSkillLeveledUp(int level) {
        }

    }

    [SkillLevelModifier("Glaive")]
    class HuntressGlaiveSkillModifier : TypedBaseSkillModifier<ThrowGlaive> {

        static HuntressGlaiveSkillModifier() {
            R2API.LanguageAPI.Add("HUNTRESS_SECONDARY_DESCRIPTION", "Throw a seeking glaive that bounces up to <style=cIsDamage>6 (+2)</style> times for <style=cIsDamage>250% damage</style>. Damage increases by <style=cIsDamage>10% (+2.5%)</style> per bounce.");
        }

        private static readonly float origDamageCoefficientPerBounce = ThrowGlaive.damageCoefficientPerBounce;
        private static readonly int origGlaiveBounceCount = 6;
        private static readonly float origGlaiveBounceRange = 35f;

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            Logger.Debug("OnSkillLeveledUp(level: {0})", level);
            Logger.Debug("Glaive stats - max bounces: {0}, damage coefficient: {1}, glaiveBounceRange: {2}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce, ThrowGlaive.glaiveBounceRange);
            ThrowGlaive.maxBounceCount = AdditiveScaling(origGlaiveBounceCount, 2, level);
            ThrowGlaive.damageCoefficientPerBounce = AdditiveScaling(origDamageCoefficientPerBounce, 0.025f, level);
            ThrowGlaive.glaiveBounceRange = AdditiveScaling(origGlaiveBounceRange, 10, level);
            Logger.Debug("Glaive stats - max bounces: {0}, damage coefficient: {1}, glaiveBounceRange: {2}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce, ThrowGlaive.glaiveBounceRange);
        }

    }

    [SkillLevelModifier("ArrowRain")]
    class HuntressArrowRainSkillModifier : TypedBaseSkillModifier<ArrowRain> {

        static HuntressArrowRainSkillModifier() {
            R2API.LanguageAPI.Add("HUNTRESS_SPECIAL_DESCRIPTION", "<style=cIsUtility>Teleport</style> into the sky. Target a <style=cIsDamage>7.5 unit (+2.5)</style> radius area to rain arrows, <style=cIsUtility>slowing</style> all enemies and dealing <style=cIsDamage>225% (+%25) damage per second</style>.");
        }

        private static readonly float origArrowRainRadius = 7.5f;
        private static readonly float origDamageCoefficient = ArrowRain.damageCoefficient;

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            ArrowRain.arrowRainRadius = AdditiveScaling(origArrowRainRadius, 2.5f, level);
            ArrowRain.damageCoefficient = AdditiveScaling(origDamageCoefficient, 0.25f, level);

            ArrowRain.projectilePrefab.transform.localScale = Vector3.one * ArrowRain.arrowRainRadius * 2;
            Logger.Debug("ArrowRain stats - arrowRainRadius: {0}, damageCoefficient: {1}, prefabScale {2}", ArrowRain.arrowRainRadius, ArrowRain.damageCoefficient, ArrowRain.projectilePrefab.transform.localScale);
        }

    }

    [SkillLevelModifier("Blink")]
    class HuntressBlinkSkillModifier : BaseSkillModifer {
        public override int MaxLevel {
            get { return 4; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() {
                typeof(BlinkState),
                typeof(MiniBlinkState)
            };
        }

        public override void OnSkillLeveledUp(int level) {

        }
        public override void OnSkillEnter(BaseState skillState, int level) {

        }

        public override void OnSkillExit(BaseState skillState, int level) {
            float duration = AdditiveScaling(0.0f, 1.5f, level);
            if (skillState is MiniBlinkState) {
                duration /= 2f;
            }
            if (duration > 0) { 
                this.CharacterBody?.AddTimedBuff(BuffIndex.FullCrit, duration);
            }
        }
    }

}