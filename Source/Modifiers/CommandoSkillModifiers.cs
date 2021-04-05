using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EntityStates.Commando;
using EntityStates.Commando.CommandoWeapon;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FirePistol", typeof(FirePistol2))]
    class CommandoFirePistolSkillModifier : SimpleSkillModifier<FirePistol2> {

        public override void OnSkillEnter(FirePistol2 skillState, int level) {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            FirePistol2.baseDuration = MultScaling(0.2f, -0.20f, level);
            FirePistol2.recoilAmplitude = MultScaling(1.5f, -0.1f, level);
            FirePistol2.spreadBloomValue = MultScaling(0.3f, -0.1f, level);
        }

    }

    [SkillLevelModifier("FireFMJ", typeof(FireFMJ))]
    class CommandoFMJSkillModifier : SimpleSkillModifier<FireFMJ> {

        public override void OnSkillEnter(FireFMJ skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("recoilAmplitude: {0},s damageCoefficient: {1}", skillState.recoilAmplitude, skillState.damageCoefficient);
            skillState.projectilePrefab.transform.localScale = new Vector3(2.90f, 2.19f, 3.86f) * AdditiveScaling(1, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple)) {
                projectileSimple.velocity = MultScaling(120f, 0.3f, level);
            }
            skillState.recoilAmplitude = MultScaling(skillState.recoilAmplitude, 0.1f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.3f, level);
        }

    }

    [SkillLevelModifier("FireShotgunBlast", typeof(FireShotgunBlast))]
    class CommandoShotgunBlastSkillModifier : SimpleSkillModifier<FireShotgunBlast> {

        public override void OnSkillEnter(FireShotgunBlast skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("procCoefficient: {0}, damageCoefficient: {1}, maxDistance: {2}", skillState.procCoefficient, skillState.damageCoefficient, skillState.maxDistance);
            skillState.bulletCount = (int) MultScaling(skillState.bulletCount, 0.3f, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.20f, level);
        }
    }

    [SkillLevelModifier("Roll", typeof(DodgeState))]
    class CommandoRollSkillModifier : SimpleSkillModifier<DodgeState> {

        public override void OnSkillEnter(DodgeState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            float duration = AdditiveScaling(0, 0.75f, level);
            if (duration > 0) {
                Logger.Debug("adding buff for {0} seconds", duration);
                var immuneBuffIndex = BuffCatalog.FindBuffIndex("Immune");
                skillState.outer.commonComponents.characterBody.AddTimedBuff(immuneBuffIndex, duration);
            }
        }
    }

    [SkillLevelModifier("Slide", typeof(SlideState))]
    class CommandoDiveSkillModifier : SimpleSkillModifier<SlideState> {

        public override void OnSkillEnter(SlideState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            float duration = AdditiveScaling(0f, 0.75f, level);
            if (duration > 0) {
                Logger.Debug("adding buff for {0} seconds", duration);
                var energizedBuffIndex = BuffCatalog.FindBuffIndex("Energized");
                skillState.outer.commonComponents.characterBody.AddTimedBuff(energizedBuffIndex, duration);
            }
        }
    }

    [SkillLevelModifier(new string[] { "Barrage", "Death Blossom" }, typeof(FireBarrage))]
    class CommandoBarrageSkillModifier : SimpleSkillModifier<FireBarrage> {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("Barrage - baseBulletCount: {0}, baseDurationBetweenShots: {1}, totalDuration: {2}, bulletRadius: {3}", FireBarrage.baseBulletCount, FireBarrage.baseDurationBetweenShots, FireBarrage.totalDuration, FireBarrage.bulletRadius);

            FireBarrage.baseBulletCount = (int) MultScaling(6, 0.3f, level);
            FireBarrage.baseDurationBetweenShots = MultScaling(0.12f, -0.20f, level);
            FireBarrage.totalDuration = FireBarrage.baseBulletCount * FireBarrage.baseDurationBetweenShots + 0.3f;
            // FireBarrage.baseDurationBetweenShots = AdditiveScaling(0.12f, -0.01f, level);
        }

    }

    [SkillLevelModifier(new string[] { "ThrowGrenade", "Carpet Bomb" }, typeof(ThrowGrenade))]
    class CommandoGrenadeSkillModifier : SimpleSkillModifier<ThrowGrenade> {

        public override void OnSkillEnter(ThrowGrenade skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("force: {0}, damageCoefficient: {1}", skillState.force, skillState.damageCoefficient);
            skillState.force = MultScaling(skillState.force, 0.2f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = MultScaling(11, 0.20f, level);
            }
        }
    }
}