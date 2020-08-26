using System;
using System.Collections.Generic;
using System.Text;

using RoR2;
using RoR2.Skills;

using EntityStates;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;
using EntityStates.Treebot.TreebotFlower;
using System.Runtime.InteropServices;
using RoR2.Projectile;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireSyringe", typeof(FireSyringe))]
    class TreebotSyringeSkillModifier : SimpleSkillModifier<FireSyringe> {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            FireSyringe.projectileCount = AdditiveScaling(3, 1, level);
            FireSyringe.totalYawSpread = AdditiveScaling(1, 0.1f, level);
            FireSyringe.damageCoefficient = MultScaling(0.8f, 0.1f, level);
        }

    }

    [SkillLevelModifier("AimMortarRain", typeof(AimMortarRain))]
    class TreebotMortarRainSkillModifier : SimpleSkillModifier<AimMortarRain> {

        public override void OnSkillEnter(AimMortarRain skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
            var radius = MultScaling(7, 0.2f, level);
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion impactExplosion)) {
                impactExplosion.blastRadius = radius;
            }
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileDotZone damageOverTimeZone)) {
                damageOverTimeZone.lifetime = MultScaling(3, 0.1f, level);
            }
            if(skillState.projectilePrefab.TryGetComponent(out HitBoxGroup hitBoxGroup)) {
                foreach(var hitbox in hitBoxGroup.hitBoxes) {
                    hitbox.transform.localScale = Vector3.one * 1.4f * radius;
                }
            }
            var expanderTransform = skillState.projectilePrefab.transform.Find("Expander");
            if(expanderTransform != null) {
                expanderTransform.transform.localScale = Vector3.one * radius;
            }
            var activeFXTransform = skillState.projectilePrefab.transform.Find("ActiveFX");
            if(activeFXTransform != null) {
                var bulletCenterTransform = activeFXTransform.Find("BulletCenter");
                if(bulletCenterTransform != null) {
                    bulletCenterTransform.transform.localScale = Vector3.one * radius;
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }


    }

    [SkillLevelModifier("AimMortar2", typeof(FireMortar2))]
    class TreebotMortar2SkillModifier : SimpleSkillModifier<AimMortar2> {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            var radius = MultScaling(5, 0.2f, level);
            if(FireMortar2.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = radius;
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
            var expanderTransform = FireMortar2.projectilePrefab.transform.Find("Expander");
            if(expanderTransform != null) {
                expanderTransform.transform.localScale = Vector3.one * radius;
            }
        }

    }

    [SkillLevelModifier("SonicBoom", typeof(FireSonicBoom))]
    class TreebotSonicBoomSkillModifier : SimpleSkillModifier<FireSonicBoom> {

        public override void OnSkillEnter(FireSonicBoom skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.30f, level);
            skillState.fieldOfView = MultScaling(skillState.fieldOfView, 0.20f, level);
            skillState.slowDuration = MultScaling(skillState.slowDuration, 0.20f, level);            
        }

    }

    [SkillLevelModifier("SonicBoom2", typeof(FirePlantSonicBoom))]
    class TreebotPlantSonicBoomSkillModifier : SimpleSkillModifier<FirePlantSonicBoom> {
        public override void OnSkillEnter(FirePlantSonicBoom skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.20f, level);
            skillState.fieldOfView = MultScaling(skillState.fieldOfView, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            FirePlantSonicBoom.damageCoefficient = MultScaling(5.5f, 0.20f, level);
        }
    }

    [SkillLevelModifier(new string[] { "FireFlower2", "Chaotic Growth" }, typeof(TreebotFlower2Projectile))]
    class TreebotFlowerSkillModifier : SimpleSkillModifier<TreebotFlower2Projectile> {

        public override void OnSkillEnter(TreebotFlower2Projectile skillState, int level) {
            base.OnSkillEnter(skillState, level);
            var areaIndicatorTransform = skillState.outer.commonComponents.modelLocator?.modelBaseTransform?.Find("AreaIndicator");
            if(areaIndicatorTransform != null) {
                areaIndicatorTransform.localScale = Vector3.one * TreebotFlower2Projectile.radius;
            }
            if(skillState.outer.TryGetComponent(out ProjectileDamage projectileDamage)) {
                projectileDamage.damage = MultScaling(2, 0.2f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            TreebotFlower2Projectile.radius = MultScaling(15, 0.20f, level);
            TreebotFlower2Projectile.healPulseCount = (int)MultScaling(16, 0.1f, level);
        }

    }
}

