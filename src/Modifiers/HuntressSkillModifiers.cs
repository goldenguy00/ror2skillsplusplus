using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using UnityEngine;

namespace Skills.Modifiers {

    [SkillLevelModifier("Glaive")]
    class HuntressGlaiveSkillModifier : BaseSkillModifier<ThrowGlaive> {

        private static readonly float origDamageCoefficientPerBounce = ThrowGlaive.damageCoefficientPerBounce;
        private static readonly int origGlaiveBounceCount = 6;
        private static readonly float origGlaiveBounceRange = 35f;        

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level) {
            ThrowGlaive.maxBounceCount = AdditiveScaling(origGlaiveBounceCount, 2, level);
            ThrowGlaive.damageCoefficientPerBounce = AdditiveScaling(origDamageCoefficientPerBounce, 0.05f, level);
            ThrowGlaive.glaiveBounceRange = AdditiveScaling(origGlaiveBounceRange, 10, level);
            Debug.LogFormat("Glaive stats - max bounces: {0}, damage coefficient: {1}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce);
        }

    }

    [SkillLevelModifier("ArrowRain")]
    class HuntressArrowRainSkillModifier : BaseSkillModifier<ArrowRain> {

        private static readonly float origArrowRainRadius = 15f;
        private static readonly float origDamageCoefficient = ArrowRain.damageCoefficient;
        
        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level) {
            ArrowRain.arrowRainRadius = AdditiveScaling(origArrowRainRadius, 5f, level);
            ArrowRain.damageCoefficient = AdditiveScaling(origDamageCoefficient, 0.2f, level);

            ArrowRain.projectilePrefab.transform.localScale = Vector3.one * ArrowRain.arrowRainRadius * 2;
            Debug.LogFormat("Glaive stats - arrowRainRadius: {0}, damageCoefficient: {1}, prefabScale {2}", ArrowRain.arrowRainRadius, ArrowRain.damageCoefficient, ArrowRain.projectilePrefab.transform.localScale);
        }

    }

    [SkillLevelModifier("Blink")]
    class HuntressBlinkSkillModifier : NoopSkillModifier { }

    [SkillLevelModifier("FireSeekingArrow")]
    class HuntressSkeeingArrowSkillModifier : NoopSkillModifier { }
}
