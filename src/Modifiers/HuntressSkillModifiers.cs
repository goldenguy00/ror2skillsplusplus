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
        private static readonly int origGlaiveBounceCount = ThrowGlaive.maxBounceCount;

        public override int MaxLevel() {
            return 5;
        }

        public override void ApplyChanges(SkillDef skillDef, int level) {
            ThrowGlaive.maxBounceCount = origGlaiveBounceCount * level;
            // ThrowGlaive.damageCoefficientPerBounce = origDamageCoefficientPerBounce * level;
        }

    }

    [SkillLevelModifier("ArrowRain")]
    class HuntressArrowRainSkillModifier : BaseSkillModifier {

        private static readonly float origArrowRainRadius = ArrowRain.arrowRainRadius;
        private static readonly float origDamageCoefficient = ArrowRain.damageCoefficient;

        public override int MaxLevel() {
            return 3;
        }

        public override void ApplyChanges(SkillDef skillDef, int level) {
            ArrowRain.arrowRainRadius = origArrowRainRadius * level;
            ArrowRain.damageCoefficient = origDamageCoefficient * level;
            ArrowRain.projectilePrefab.transform.localScale = Vector3.one * level;
        }

    }

    [SkillLevelModifier("Blink")]
    class HuntressBlinkSkillModifier : NoopSkillModifier { }

    [SkillLevelModifier("FireSeekingArrow")]
    class HuntressSkeeingArrowSkillModifier : NoopSkillModifier { }
}
