using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
using EntityStates.Commando;
using EntityStates.Commando.CommandoWeapon;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FirePistol")]
    class CommandoFirePistolSkillModifier : TypedBaseSkillModifier<FirePistol2> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FirePistol2 skillState, int level) {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FirePistol2");
            Logger.Debug(FirePistol2.baseDuration);
            Logger.Debug(FirePistol2.recoilAmplitude);
            FirePistol2.baseDuration = MultScaling(0.2f, -0.20f, level);
            FirePistol2.recoilAmplitude = MultScaling(1.5f, -0.15f, level);            
        }

    }

    [SkillLevelModifier("FireFMJ")]
    class CommandoFMJSkillModifier : TypedBaseSkillModifier<FireFMJ> {

        static CommandoFMJSkillModifier() {
            // R2API.LanguageAPI.Add("COMMANDO_SECONDARY_DESCRIPTION", "Fire a piercing bullet that hits all enemies in a line for <style=cIsDamage>300% damage</style>. Projectile travels faster at higher levels.");
        }

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireFMJ skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("recoilAmplitude: {0},s damageCoefficient: {1}", skillState.recoilAmplitude, skillState.damageCoefficient);
            skillState.projectilePrefab.transform.localScale = new Vector3(2.90f, 2.19f, 3.86f) * AdditiveScaling(1, 0.5f, level);
            if (skillState.projectilePrefab.TryGetComponent<ProjectileSimple>(out ProjectileSimple projectileSimple)) {
                projectileSimple.velocity = MultScaling(120f, 0.5f, level);
            }
            skillState.recoilAmplitude = MultScaling(skillState.recoilAmplitude, 0.5f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.5f, level);
        }

    }

    [SkillLevelModifier("FireShotgunBlast")]
    class CommandoShotgunBlastSkillModifier : TypedBaseSkillModifier<FireShotgunBlast> {
        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireShotgunBlast skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("procCoefficient: {0}, damageCoefficient: {1}, maxDistance: {2}", skillState.procCoefficient, skillState.damageCoefficient, skillState.maxDistance);
            skillState.bulletCount = MultScaling(skillState.bulletCount, 0.5f, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.25f, level);
        }
    }

    [SkillLevelModifier("Roll")]
    class CommandoRollSkillModifier : TypedBaseSkillModifier<DodgeState> {

        public override int MaxLevel {
            get { return 4; }
        }
        public override void OnSkillEnter(DodgeState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("duration: {0}", skillState.duration);
            float duration = skillState.duration * AdditiveScaling(0, 0.75f, level);
            if (duration > 0) {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BuffIndex.Immune, duration);
            }
        }
    }

    [SkillLevelModifier("Slide")]
    class CommandoDiveSkillModifier : TypedBaseSkillModifier<SlideState> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(SlideState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            float duration = SlideState.slideDuration * AdditiveScaling(0f, 0.75f, level);
            if (duration > 0) {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BuffIndex.Energized, duration);
            }
        }
    }

    [SkillLevelModifier("Barrage")]
    class CommandoBarrageSkillModifier : TypedBaseSkillModifier<FireBarrage> {

        public override int MaxLevel {
            get { return 4; }
        }
        public override void OnSkillEnter(FireBarrage skillState, int level) {
            base.OnSkillEnter(skillState, level);
            //skillState.
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("Barrage - baseBulletCount: {0}, baseDurationBetweenShots: {1}, totalDuration: {2}, bulletRadius: {3}", FireBarrage.baseBulletCount, FireBarrage.baseDurationBetweenShots, FireBarrage.totalDuration, FireBarrage.bulletRadius);
            
            FireBarrage.baseBulletCount = MultScaling(6, 0.5f, level);
            FireBarrage.totalDuration = MultScaling(1f, 0.25f, level);
            FireBarrage.baseDurationBetweenShots = 1 / MultScaling(1 / 0.12f, 0.25f, level);
            // FireBarrage.baseDurationBetweenShots = AdditiveScaling(0.12f, -0.01f, level);
        }

    }

    [SkillLevelModifier("ThrowGrenade")]
    class CommandoGrenadeSkillModifier : TypedBaseSkillModifier<ThrowGrenade> {
        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(ThrowGrenade skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("force: {0}, damageCoefficient: {1}", skillState.force, skillState.damageCoefficient);
            skillState.force = MultScaling(skillState.force, 0.5f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.33f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = MultScaling(11, 0.25f, level);
            }
        }
    }
}
