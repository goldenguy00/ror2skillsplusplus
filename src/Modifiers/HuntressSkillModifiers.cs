using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using R2API.AssetPlus;
using UnityEngine;
using RoR2.UI;
using RoR2;

namespace Skills.Modifiers {

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
    class HuntressBlinkSkillModifier : NoopSkillModifier { }

    [SkillLevelModifier("FireSeekingArrow")]
    class HuntressSkeeingArrowSkillModifier : NoopSkillModifier { }
}
