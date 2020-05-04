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
    class ArtificerFireBoltSkillModifier : TypedBaseSkillModifier<FireFireBolt> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
            SkillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            SkillDef.baseRechargeInterval = AdditiveScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("NovaBomb")]
    class MageNovaBombSkillModifier : BaseSkillModifer {



        public int MaxLevel {
            get { return 3; }
        }

        public override void OnSkillLeveledUp(int level) {

        }

    }

    [SkillLevelModifier("IceBomb")]
    class MangeIceBombSkillModifier : MageNovaBombSkillModifier { }

    [SkillLevelModifier("Wall")]
    class MageWallSkillModifier : TypedBaseSkillModifier<PrepWall> {

        public override int MaxLevel {
            get { return 1; }
        }

        public override void OnSkillLeveledUp(int level) {
            //float scale = AdditiveScaling(1, 0.5f, level);
            ////PrepWall.areaIndicatorPrefab.transform.localScale = Vector3.one * scale;
            ////PrepWall.projectilePrefab.transform.localScale = Vector3.one * scale;
        }
    
    }

    [SkillLevelModifier("Flamethrower")]
    class MageFlamethrowerSkillModifier : TypedBaseSkillModifier<Flamethrower> {

        private float baseRadius = 2f;
        private float totalDamageCoefficient = 1.2f;

        private float baseMaxDistance;

        public override int MaxLevel {
            get { return 4; }
        }

        protected override void OnSkillWillBeUsed(Flamethrower skillState, int level) {
            base.OnSkillWillBeUsed(skillState, level);
            Logger.Debug(skillState.maxDistance);
            baseMaxDistance = skillState.maxDistance;
            skillState.maxDistance = AdditiveScaling(baseMaxDistance, baseMaxDistance, level);

            Logger.Debug(skillState.flamethrowerEffectPrefab.transform.localScale);
            skillState.flamethrowerEffectPrefab.transform.localScale = new Vector3(Flamethrower.radius, Flamethrower.radius, AdditiveScaling(1, 1, level));
            //if (skillState.flamethrowerEffectPrefab.TryGetComponent(out DestroyOnTimer timer)) {
            //    timer.duration = Flamethrower.baseFlamethrowerDuration;
            //}
            Logger.Debug(skillState.maxDistance);
        }

        public override void OnSkillLeveledUp(int level) {
            Flamethrower.radius = AdditiveScaling(baseRadius, baseRadius * 0.5f, level);
            //Flamethrower.baseFlamethrowerDuration = AdditiveScaling(baseFlamethrowerDuration, 2, level);
            Flamethrower.totalDamageCoefficient = AdditiveScaling(totalDamageCoefficient, 0.2f, level);
            Logger.Debug("Flamethrower stats - baseFlamethrowerDuration: {0}, totalDamageCoefficient: {1}, radius: {2}", Flamethrower.baseFlamethrowerDuration, Flamethrower.totalDamageCoefficient, Flamethrower.radius);
        }

    }
}
