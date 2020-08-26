using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using UnityEngine;

using EntityStates;
using EntityStates.Captain;
using EntityStates.Captain.Weapon;
using EntityStates.CaptainDefenseMatrixItem;
using EntityStates.CaptainSupplyDrop;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;

using SkillsPlusPlus.Modifiers;
using R2API.Utils;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("CaptainShotgun", typeof(FireCaptainShotgun), typeof(ChargeCaptainShotgun))]
    class CaptainShotgunSkillModifier : SimpleSkillModifier<FireCaptainShotgun> {

        public override void OnSkillEnter(FireCaptainShotgun skillState, int level) {
            base.OnSkillEnter(skillState, level);
            skillState.bulletCount = (int)MultScaling(skillState.bulletCount, 0.2f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
        }
    }

    [SkillLevelModifier("CaptainTazer", typeof(FireTazer))]
    class CaptainTaserSkillModifier : SimpleSkillModifier<FireTazer> {

        public override void OnSkillEnter(FireTazer skillState, int level) {
            base.OnSkillEnter(skillState, level);

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(FireTazer.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = MultScaling(2, 0.4f, level);
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
        }

    }

    [SkillLevelModifier(new string[] { "CaptainPrepAirstrike", "21-Probe Salute" }, typeof(CallAirstrike1), typeof(CallAirstrike2), typeof(CallAirstrike3))]
    class CaptainAirstrikeSkillModifier : SimpleSkillModifier<CallAirstrikeBase> {

        public override void OnSkillEnter(CallAirstrikeBase skillState, int level) {
            base.OnSkillEnter(skillState, level);
            var radius = MultScaling(8, 0.2f, level);
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                projectileImpactExplosion.blastRadius = radius;
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
            if(skillState.projectilePrefab.TryGetComponent(out ProjectileController projectileController)) {
                var expanderTransform = projectileController.ghostPrefab.transform.Find("Expander");
                if(expanderTransform != null) {
                    expanderTransform.localScale = Vector3.one * radius;
                }
            }
        }

    }

    [SkillLevelModifier(new string[] { "CaptainPrepSupplyDrop", "CaptainSupplyDropDepleted" }, typeof(DeployState), typeof(HealZoneMainState), typeof(ShockZoneMainState), typeof(HackingMainState), typeof(HackingInProgressState), typeof(EquipmentRestockMainState))]
    class CaptainSupplyDropHealingSkillModifier : BaseSkillModifier {

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is DeployState deploying) {
                var modelLocator = deploying.outer?.commonComponents.modelLocator;
                if(modelLocator != null) {
                    var indicatorTransform = modelLocator.modelTransform?.Find("Indicator");
                    Logger.Debug(indicatorTransform);
                    if(indicatorTransform != null) {
                        indicatorTransform.localScale = Vector3.one * HackingMainState.baseRadius / 2;
                        if(indicatorTransform.TryGetComponent(out ObjectScaleCurve objectScaleCurve)) {
                            objectScaleCurve.baseScale = indicatorTransform.localScale;
                        }
                    }
                }
            }
            if(skillState is EquipmentRestockMainState equipmentRestock) {
                equipmentRestock.activationCost = 100 / AdditiveScaling(3, 1, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(HealZoneMainState.healZonePrefab.TryGetComponent(out HealingWard healingWard)) {
                healingWard.radius = MultScaling(10, 0.2f, level);
            }
            ShockZoneMainState.shockRadius = MultScaling(10, 0.3f, level);

            HackingMainState.baseRadius = MultScaling(10, 0.2f, level);
            HackingInProgressState.baseDuration = MultScaling(15, -0.2f, level);
        }
        
    }
}
