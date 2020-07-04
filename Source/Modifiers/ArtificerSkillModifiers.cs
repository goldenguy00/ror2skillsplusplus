using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using UnityEngine;
using RoR2;
using RoR2.Projectile;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireFirebolt")]
    class ArtificerFireBoltSkillModifier : SimpleSkillModifier<FireFireBolt> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
            skillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            skillDef.baseRechargeInterval = MultScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("FireLightningBolt")]
    class ArtificerLightningBoltSkillModifier : SimpleSkillModifier<FireLightningBolt> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
            skillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            skillDef.baseRechargeInterval = MultScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("NovaBomb")]
    class MageNovaBombSkillModifier : SimpleSkillModifier<ChargeNovabomb> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(ChargeNovabomb skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("baseChargeDuration: {0}, maxDamageCoefficient: {1}, maxRadius: {2}", skillState.baseChargeDuration, skillState.maxDamageCoefficient, skillState.maxRadius);
            skillState.baseChargeDuration = MultScaling(skillState.baseChargeDuration, 0.5f, level);
            skillState.minDamageCoefficient = MultScaling(skillState.minDamageCoefficient, 0.25f, level);
            skillState.maxDamageCoefficient = MultScaling(skillState.maxDamageCoefficient, 0.75f, level); // 50% to keep up with charge duration + 25% damage bonus
            skillState.maxRadius = MultScaling(skillState.maxRadius, 0.5f, level);
            skillState.force = MultScaling(skillState.force, 0.5f, level);
            Logger.Debug("baseChargeDuration: {0}, maxDamageCoefficient: {1}, maxRadius: {2}", skillState.baseChargeDuration, skillState.maxDamageCoefficient, skillState.maxRadius);
        }

    }

    [SkillLevelModifier("IceBomb")]
    class MageIceBombSkillModifier : MageNovaBombSkillModifier {

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(ChargeIcebomb) };
        }

    }

    [SkillLevelModifier("Wall")]
    class MageWallSkillModifier : SimpleSkillModifier<PrepWall> {

        public override int MaxLevel {
            get { return 4; }
        }
        public override void OnSkillEnter(PrepWall skillState, int level) {
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);            
            PrepWall.damageCoefficient = AdditiveScaling(1, 0.5f, level);
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);
            PrepWall.damageCoefficient = AdditiveScaling(1, 0.5f, level);

            foreach (Component component in PrepWall.projectilePrefab.GetComponents<Component>()) {
                Logger.Debug(component);
            }

            if(PrepWall.projectilePrefab.TryGetComponent(out ProjectileCharacterController projectileCharacterController)) {
                projectileCharacterController.lifetime = MultScaling(0.3f, 0.25f, level);
                Logger.Debug("projectileCharacterController.lifetime: {0}", projectileCharacterController.lifetime);
            }
            
            float newXScale = MultScaling(23.69f, 0.25f, level);
            PrepWall.areaIndicatorPrefab.transform.localScale = new Vector3(newXScale, PrepWall.areaIndicatorPrefab.transform.localScale.y, PrepWall.areaIndicatorPrefab.transform.localScale.z);
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);
        }
    
    }

    [SkillLevelModifier("Flamethrower", "Dragon's Breath")]
    class MageFlamethrowerSkillModifier : SimpleSkillModifier<Flamethrower> {

        private float baseRadius = 2f;
        private float totalDamageCoefficient = 1.2f;

        private float baseMaxDistance;

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(Flamethrower skillState, int level) {
            base.OnSkillEnter(skillState, level);
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

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("Flamethrower stats - baseFlamethrowerDuration: {0}, totalDamageCoefficient: {1}, radius: {2}", Flamethrower.baseFlamethrowerDuration, Flamethrower.totalDamageCoefficient, Flamethrower.radius);
            Flamethrower.radius = AdditiveScaling(baseRadius, baseRadius * 0.5f, level);
            //Flamethrower.baseFlamethrowerDuration = AdditiveScaling(baseFlamethrowerDuration, 2, level);
            Flamethrower.totalDamageCoefficient = MultScaling(20, 0.25f, level);
            Logger.Debug("Flamethrower stats - baseFlamethrowerDuration: {0}, totalDamageCoefficient: {1}, radius: {2}", Flamethrower.baseFlamethrowerDuration, Flamethrower.totalDamageCoefficient, Flamethrower.radius);
        }

    }


    [SkillLevelModifier("FlyUp", "Antimatter Surge")]
    class MageFlyUpSkillModifier : SimpleSkillModifier<FlyUpState> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FlyUpState skillState, int level) {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FlyUpState stats - duration: {0}, blastAttackDamageCoefficient: {1}, blastAttackRadius: {2}, blastAttackProcCoefficient: {3}", FlyUpState.duration, FlyUpState.blastAttackDamageCoefficient, FlyUpState.blastAttackRadius, FlyUpState.blastAttackProcCoefficient);
            FlyUpState.blastAttackDamageCoefficient = MultScaling(8, 0.25f, level);
            FlyUpState.blastAttackRadius = MultScaling(8, 0.5f, level);
            Logger.Debug("FlyUpState stats - duration: {0}, blastAttackDamageCoefficient: {1}, blastAttackRadius: {2}, blastAttackProcCoefficient: {3}", FlyUpState.duration, FlyUpState.blastAttackDamageCoefficient, FlyUpState.blastAttackRadius, FlyUpState.blastAttackProcCoefficient);
        }

    }
}
