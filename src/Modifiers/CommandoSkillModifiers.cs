using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using UnityEngine;
using RoR2;
using EntityStates.Commando;
using EntityStates.Commando.CommandoWeapon;

namespace Skills.Modifiers {

    [SkillLevelModifier("FirePistol")]
    class CommandoFirePistolSkillModifier : BaseSkillModifier<FirePistol2> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level) {
            Debug.Log("FirePistol2");
            Debug.Log(FirePistol2.baseDuration);
            Debug.Log(FirePistol2.recoilAmplitude);
            FirePistol2.baseDuration = AdditiveScaling(0.2f, -0.025f, level);
            FirePistol2.recoilAmplitude = AdditiveScaling(1.5f, -0.375f, level);
        }

    }

    [SkillLevelModifier("FireFMJ")]
    class CommandoFMJSkillModifier : BaseSkillModifier<FireFMJ> {

        static CommandoFMJSkillModifier() {
            R2API.LanguageAPI.Add("COMMANDO_SECONDARY_DESCRIPTION", "Fire a piercing bullet that hits all enemies in a line for <style=cIsDamage>300% damage</style>. Projectile travels faster at higher levels.");
        }

        public override int MaxLevel {
            get { return 3; }
        }

        protected override void OnSkillWillBeUsed(FireFMJ skillState, int level) {
            base.OnSkillWillBeUsed(skillState, level);
            skillState.projectilePrefab.transform.localScale = new Vector3(2.90f, 2.19f, 3.86f) * AdditiveScaling(1, 0.5f, level);
        }

        public override void OnSkillLeveledUp(int level) {
            SkillDef.baseMaxStock = AdditiveScaling(1, 1, level);
        }

    }

    [SkillLevelModifier("Roll")]
    class CommandoRollSkillModifier : BaseSkillModifier<CombatDodge> {

        public override int MaxLevel {
            get { return 3; }
        }

        public override void OnSkillLeveledUp(int level) {

        }
    
    }

    [SkillLevelModifier("Barrage")]
    class CommandoBarrageSkillModifier : BaseSkillModifier<FireBarrage> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level) {
            Debug.Log("Barrage");
            Debug.Log(FireBarrage.baseBulletCount);
            Debug.Log(FireBarrage.baseDurationBetweenShots);
            Debug.Log(FireBarrage.bulletRadius);
            FireBarrage.baseBulletCount = AdditiveScaling(6, 4, level);
            FireBarrage.baseDurationBetweenShots = AdditiveScaling(0.12f, -0.01f, level);
            FireBarrage.bulletRadius = AdditiveScaling(1.5f, 0.5f, level);
            // FireBarrage.totalDuration = AdditiveScaling(2f, 0.5f, level);
        }

    }
}
