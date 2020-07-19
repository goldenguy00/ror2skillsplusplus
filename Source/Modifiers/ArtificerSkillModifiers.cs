using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
using EntityStates;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireFirebolt")]
    class ArtificerFireBoltSkillModifier : SimpleSkillModifier<FireFireBolt> {

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

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
            skillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            skillDef.baseRechargeInterval = MultScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("NovaBomb")]
    class MageNovaBombSkillModifier : BaseSkillModifier {

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { typeof(ChargeNovabomb) };
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is ChargeNovabomb chargeBomb) {
                Logger.Debug("baseChargeDuration: {0}, maxDamageCoefficient: {1}, maxRadius: {2}", chargeBomb.baseChargeDuration, chargeBomb.maxDamageCoefficient, chargeBomb.maxRadius);
                chargeBomb.baseChargeDuration = MultScaling(chargeBomb.baseChargeDuration, 0.5f, level);
                chargeBomb.minDamageCoefficient = MultScaling(chargeBomb.minDamageCoefficient, 0.25f, level);
                chargeBomb.maxDamageCoefficient = MultScaling(chargeBomb.maxDamageCoefficient, 0.75f, level); // 50% to keep up with charge duration + 25% damage bonus
                chargeBomb.maxRadius = MultScaling(chargeBomb.maxRadius, 0.5f, level);
                chargeBomb.force = MultScaling(chargeBomb.force, 0.5f, level);
                Logger.Debug("baseChargeDuration: {0}, maxDamageCoefficient: {1}, maxRadius: {2}", chargeBomb.baseChargeDuration, chargeBomb.maxDamageCoefficient, chargeBomb.maxRadius);
            }
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

        public override void OnSkillEnter(Flamethrower skillState, int level) {
            base.OnSkillEnter(skillState, level);
            Logger.Debug(skillState.maxDistance);
            skillState.maxDistance = AdditiveScaling(skillState.maxDistance, skillState.maxDistance, level);

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
