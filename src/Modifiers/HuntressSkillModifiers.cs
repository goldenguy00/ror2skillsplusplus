using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using EntityStates.Huntress.Weapon;
using UnityEngine;

namespace Skills.Modifiers {

    [SkillLevelModifier("Glaive")]
    class HuntressGlaiveSkillModifier : BaseSkillModifier {

        private static readonly float origDamageCoefficientPerBounce = ThrowGlaive.damageCoefficientPerBounce;
        private static readonly int origGlaiveBounceCount = 6;
        private static readonly float origGlaiveBounceRange = 35f;
        

        public override int MaxLevel() {
            return 4;
        }

        public override void ApplyChanges(SkillDef skillDef, int level) {
            ThrowGlaive.maxBounceCount = (int) LinearScaling(origGlaiveBounceCount, 2, level);
            ThrowGlaive.damageCoefficientPerBounce = LinearScaling(origDamageCoefficientPerBounce, 0.05f, level);
            ThrowGlaive.glaiveBounceRange = LinearScaling(origGlaiveBounceRange, 10, level);
            Debug.LogFormat("Glaive stats - max bounces: {0}, damage coefficient: {1}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce);
        }

    }

    [SkillLevelModifier("ArrowRain")]
    class HuntressArrowRainSkillModifier : BaseSkillModifier {

        private static readonly float origArrowRainRadius = 15f;
        private static readonly float origDamageCoefficient = ArrowRain.damageCoefficient;

        public override int MaxLevel() {
            return 4;
        }

        public override void ApplyChanges(SkillDef skillDef, int level) {
            ArrowRain.arrowRainRadius = LinearScaling(origArrowRainRadius, 5f, level);
            ArrowRain.damageCoefficient = LinearScaling(origDamageCoefficient, 0.5f, level);

            ArrowRain.projectilePrefab.transform.localScale = Vector3.one * ArrowRain.arrowRainRadius * 2;
            Debug.LogFormat("Glaive stats - arrowRainRadius: {0}, damageCoefficient: {1}, prefabScale {2}", ArrowRain.arrowRainRadius, ArrowRain.damageCoefficient, ArrowRain.projectilePrefab.transform.localScale);
        }

    }

    [SkillLevelModifier("Blink")]
    class HuntressBlinkSkillModifier : NoopSkillModifier { }

    [SkillLevelModifier("FireSeekingArrow")]
    class HuntressSkeeingArrowSkillModifier : NoopSkillModifier { }
}
