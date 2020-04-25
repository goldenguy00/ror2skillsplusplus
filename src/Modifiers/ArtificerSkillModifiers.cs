using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using UnityEngine;
using RoR2;

namespace Skills.Modifiers {

    [SkillLevelModifier("FireFirebolt")]
    class ArtificerFireBoltSkillModifier : BaseSkillModifier<FireFireBolt> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            SkillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            SkillDef.baseRechargeInterval = AdditiveScaling(1.3f, -0.1f, level);
        }

    }

    [SkillLevelModifier("NovaBomb")]
    class HuntressNovaBombSkillModifier : BaseSkillModifier<ChargeNovabomb> {

        public override int MaxLevel {
            get { return 3; }
        }

        public override void OnSkillLeveledUp(int level) {
            SkillDef.baseMaxStock = AdditiveScaling(1, 1, level);
        }

    }

    [SkillLevelModifier("Wall")]
    class MageWallSkillModifier : BaseSkillModifier<PrepWall> {

        public override int MaxLevel {
            get { return 3; }
        }

        public override void OnSkillLeveledUp(int level) {
            float scale = AdditiveScaling(1, 0.5f, level);
            PrepWall.areaIndicatorPrefab.transform.localScale = Vector3.one * scale;
            PrepWall.projectilePrefab.transform.localScale = Vector3.one * scale;
        }
    
    }

    [SkillLevelModifier("Flamethrower")]
    class MageFlamethrowerSkillModifier : BaseSkillModifier<Flamethrower> {

        private float baseRadius = 2f;
        private float totalDamageCoefficient = 1.2f;

        private float baseMaxDistance;

        public override int MaxLevel {
            get { return 4; }
        }

        protected override void OnSkillWillBeUsed(Flamethrower skillState, int level) {
            base.OnSkillWillBeUsed(skillState, level);
            Debug.Log(skillState.maxDistance);
            baseMaxDistance = skillState.maxDistance;
            skillState.maxDistance = AdditiveScaling(baseMaxDistance, baseMaxDistance, level);

            Debug.Log(skillState.flamethrowerEffectPrefab.transform.localScale);
            skillState.flamethrowerEffectPrefab.transform.localScale = new Vector3(Flamethrower.radius, Flamethrower.radius, AdditiveScaling(1, 1, level));
            //if (skillState.flamethrowerEffectPrefab.TryGetComponent(out DestroyOnTimer timer)) {
            //    timer.duration = Flamethrower.baseFlamethrowerDuration;
            //}
            Debug.Log(skillState.maxDistance);
        }

        public override void OnSkillLeveledUp(int level) {
            Flamethrower.radius = AdditiveScaling(baseRadius, baseRadius * 0.5f, level);
            //Flamethrower.baseFlamethrowerDuration = AdditiveScaling(baseFlamethrowerDuration, 2, level);
            Flamethrower.totalDamageCoefficient = AdditiveScaling(totalDamageCoefficient, 0.2f, level);
            Debug.LogFormat("Flamethrower stats - baseFlamethrowerDuration: {0}, totalDamageCoefficient: {1}, radius: {2}", Flamethrower.baseFlamethrowerDuration, Flamethrower.totalDamageCoefficient, Flamethrower.radius);
        }

    }
}
