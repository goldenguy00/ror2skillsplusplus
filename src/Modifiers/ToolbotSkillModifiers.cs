using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.UI;

using EntityStates;
using EntityStates.Toolbot;
using R2API.Utils;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireNailgun")]
    class ToolbotSkillModifier : TypedBaseSkillModifier<FireNailgun> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireNailgun skillState, int level) {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody) {
            base.OnSkillLeveledUp(level, characterBody);
            Logger.Debug("damageCoefficient: {0}, bulletCount: {1}, procCoefficient: {2}, baseCooldownDuration: {3}", FireNailgun.damageCoefficient, FireNailgun.bulletCount, FireNailgun.procCoefficient, FireNailgun.baseCooldownDuration);
            FireNailgun.bulletCount = AdditiveScaling(6, 2, level);
            FireNailgun.damageCoefficient = MultScaling(0.6f, 0.25f, level);
            FireNailgun.baseCooldownDuration = MultScaling(0.8f, -0.15f, level);
        }
    }

    [SkillLevelModifier("FireSpear")]
    class ToolbotSpearSkillModifier : TypedBaseSkillModifier<FireSpear> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireSpear fireSpear, int level) {
            base.OnSkillEnter(fireSpear, level);
            fireSpear.baseDuration = MultScaling(fireSpear.baseDuration, -0.25f, level);
            fireSpear.damageCoefficient = MultScaling(fireSpear.damageCoefficient, 0.25f, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody) {
            base.OnSkillLeveledUp(level, characterBody);
            Logger.Debug("damageCoefficient: {0}, bulletCount: {1}, procCoefficient: {2}, baseCooldownDuration: {3}", FireNailgun.damageCoefficient, FireNailgun.bulletCount, FireNailgun.procCoefficient, FireNailgun.baseCooldownDuration);
        }
    }

    [SkillLevelModifier("FireGrenadeLauncher")]
    class ToolbotGrenadeLauncherSkillModifier : TypedBaseSkillModifier<FireGrenadeLauncher> {

        private static readonly float stockImageInterspacing = 13.5f;

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireGrenadeLauncher skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.25f, level);
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = MultScaling(5, 0.20f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody) {
            base.OnSkillLeveledUp(level, characterBody);
            int stocks = AdditiveScaling(4, 1, level);

            SkillDef.baseMaxStock = stocks;
            
            if(this.SkillDef is ToolbotWeaponSkillDef) {
                // return;
                // instantiate a new version of the prefab because it will cause the crosshair manager to recognize a new
                // prefab and reload the crosshair displayed on screen
                GameObject crosshairPrefab = ((ToolbotWeaponSkillDef)this.SkillDef).crosshairPrefab;
                if(crosshairPrefab.TryGetComponent(out CrosshairController crosshairController)) {
                    GameObject stockCountHolderGameObject = crosshairController.gameObject.transform.Find("StockCountHolder").gameObject;
                    RectTransform stockCountHolderRectTransform = stockCountHolderGameObject.GetComponent<RectTransform>();

                    List<GameObject> stockGameObjects = new List<GameObject>();
                    GameObject stockPrefab = null;
                    for(int i = 0; i < stockCountHolderGameObject.transform.childCount; i++) {
                        if(stockPrefab == null) {
                            stockPrefab = stockCountHolderGameObject.transform.GetChild(i).gameObject;
                        }
                        stockGameObjects.Add(stockCountHolderGameObject.transform.GetChild(i).gameObject);
                    }

                    if(stockPrefab) {
                        while(stockGameObjects.Count < stocks) {
                            GameObject newStock = GameObject.Instantiate(stockPrefab);
                            newStock.transform.parent = stockCountHolderGameObject.transform;
                            RectTransform stockRectTransform = newStock.GetComponent<RectTransform>();
                            stockRectTransform.rotation = Quaternion.Euler(0, 0, 270);
                            stockRectTransform.localPosition = Vector2.zero;
                            stockRectTransform.anchorMin = Vector2.zero;
                            stockRectTransform.anchorMax = Vector2.zero;
                            stockRectTransform.offsetMin = new Vector2(1.75f, 0);
                            stockRectTransform.offsetMax = new Vector2(-1.75f, 0);
                            stockRectTransform.sizeDelta = new Vector2(-3.5f, 0);
                            stockRectTransform.ForceUpdateRectTransforms();
                            stockGameObjects.Add(newStock);
                            newStock.name = "Stock, " + stockGameObjects.Count;
                        }
                    }

                    stockCountHolderRectTransform.sizeDelta = new Vector2(stocks * stockImageInterspacing, 10);

                    CrosshairController.SkillStockSpriteDisplay[] stockDisplays = new CrosshairController.SkillStockSpriteDisplay[stocks];
                    float anchorIncrement = 1f / stocks;
                    for(int i = 0; i < Math.Min(stocks, stockGameObjects.Count); i++) {
                        stockDisplays[i] = new CrosshairController.SkillStockSpriteDisplay() {
                            target = stockGameObjects[i],
                            skillSlot = SkillSlot.Primary,
                            minimumStockCountToBeValid = i + 1,
                            maximumStockCountToBeValid = 999
                        };
                        RectTransform stockRectTransform = stockGameObjects[i].GetComponent<RectTransform>();
                        stockRectTransform.anchorMin = new Vector2(anchorIncrement * i, 0);
                        stockRectTransform.anchorMax = new Vector2(anchorIncrement * (i + 1), 1);
                        stockRectTransform.ForceUpdateRectTransforms();
                    }

                    crosshairController.skillStockSpriteDisplays = stockDisplays;                    
                }

                
                characterBody.crosshairPrefab = crosshairPrefab;
            }
        }
    }

    [SkillLevelModifier("FireBuzzsaw")]
    class ToolbotBuzzsawSkillModifier : TypedBaseSkillModifier<FireBuzzsaw> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(FireBuzzsaw fireBuzzsaw, int level) {
            base.OnSkillEnter(fireBuzzsaw, level);
            Transform modelTransform = fireBuzzsaw.outer?.commonComponents.modelLocator?.modelTransform;
            if(modelTransform) {
                HitBoxGroup buzzsawHitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Buzzsaw");
                foreach(HitBox hitBox in buzzsawHitBoxGroup.hitBoxes) {
                    hitBox.transform.localScale = new Vector3(21.04f, 21.04f, 12.04f) * MultScaling(1, 0.5f, level);
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody) {
            base.OnSkillLeveledUp(level, characterBody);
            Logger.Debug("baseFireFrequency: {0}", FireBuzzsaw.baseFireFrequency);
            FireBuzzsaw.damageCoefficientPerSecond = MultScaling(10, 0.25f, level);
            FireBuzzsaw.baseFireFrequency = MultScaling(10, 0.25f, level);
        }
    }

    [SkillLevelModifier("StunDrone")]
    class ToolbotStunDroneSkillModifier : TypedBaseSkillModifier<AimStunDrone> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(AimStunDrone aimStunDrone, int level) {
            base.OnSkillEnter(aimStunDrone, level);
            Logger.Debug("damageCoefficient: {0}, baseMinimumDuration: {1}, maxDistance: {2}", aimStunDrone.damageCoefficient, aimStunDrone.baseMinimumDuration, aimStunDrone.maxDistance);
            aimStunDrone.maxDistance = MultScaling(aimStunDrone.maxDistance, 0.5f, level);
            if(aimStunDrone.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = MultScaling(12f, 0.25f, level);
                projectileImpactExplosion.childrenCount = AdditiveScaling(5, 3, level);
                if(projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple)) {
                    projectileSimple.velocity = MultScaling(10, 0.5f, level);
                }
                if(projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileImpactExplosion clusterProjectileExplosion)) {
                    clusterProjectileExplosion.blastRadius = MultScaling(6, 0.25f, level);
                    clusterProjectileExplosion.blastDamageCoefficient = MultScaling(1, 0.25f, level);
                }
            }
        }

    }

    [SkillLevelModifier("ToolbotDash")]
    class ToolbotDashSkillModifier : TypedBaseSkillModifier<ToolbotDash> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(ToolbotDash toolbotDash, int level) {
            base.OnSkillEnter(toolbotDash, level);
            Logger.Debug("baseDuration: {0}, speedMultiplier: {2}, chargeDamageCoefficient: {5}, knockbackForce: {1}, knockbackDamageCoefficient: {3}, massThresholdForKnockback: {4}", toolbotDash.baseDuration, ToolbotDash.knockbackForce, toolbotDash.speedMultiplier, ToolbotDash.knockbackDamageCoefficient, ToolbotDash.massThresholdForKnockback, ToolbotDash.chargeDamageCoefficient);
            // baseDuration: 2, speedMultiplier: 2.2, knockbackForce: 8000, knockbackDamageCoefficient: 10, massThresholdForKnockback: 250
            toolbotDash.baseDuration = MultScaling(toolbotDash.baseDuration, 0.25f, level);
            toolbotDash.speedMultiplier = MultScaling(2.2f, 0.25f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody) {
            base.OnSkillLeveledUp(level, characterBody);
            ToolbotDash.chargeDamageCoefficient = MultScaling(10, 0.50f, level);
            ToolbotDash.knockbackDamageCoefficient = MultScaling(10, 0.50f, level);
            ToolbotDash.massThresholdForKnockback = MultScaling(250, 0.5f, level);
            ToolbotDash.knockbackForce = MultScaling(8000, 0.25f, level);
        }
    }

    [SkillLevelModifier("Swap")]
    class ToolbotEquipmentSwapSkillModifier : TypedBaseSkillModifier<ToolbotStanceSwap> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillEnter(ToolbotStanceSwap toolbotStanceSwap, int level) {
            base.OnSkillEnter(toolbotStanceSwap, level);
            Logger.Debug("baseDuration: {0}", toolbotStanceSwap.GetFieldValue<float>("baseDuration"));
            toolbotStanceSwap.SetFieldValue("baseDuration", MultScaling(0.7f, -0.25f, level));
        }

        public override void OnSkillExit(ToolbotStanceSwap skillState, int level) {
            base.OnSkillExit(skillState, level);
            float duration = AdditiveScaling(0, 1, level);
            if(duration > 0) {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BuffIndex.Energized, AdditiveScaling(0, 1, level));
            }
        }
    }
}
