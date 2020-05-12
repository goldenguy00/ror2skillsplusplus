using System;
using System.Collections.Generic;
using UnityEngine;

using RoR2;
using RoR2.CharacterAI;

using EntityStates.Engi;
using EntityStates.Engi.EngiWeapon;
using EntityStates.EngiTurret;
using RoR2.Projectile;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using EntityStates.Engi.EngiBubbleShield;
using EntityStates.EngiTurret.EngiTurretWeapon;
using EntityStates.Engi.Mine;
using EntityStates;
using R2API.Utils;
using System.Linq;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireGrenade")]
    class EngiGrenadesSkillModifier : TypedBaseSkillModifier<ChargeGrenades> {

        public override int MaxLevel {
            get { return 5; }
        }

        public override void OnSkillLeveledUp(int level) {
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", SkillDef.baseMaxStock, SkillDef.baseRechargeInterval);
            ChargeGrenades.minGrenadeCount = AdditiveScaling(2, 1, level);
            ChargeGrenades.maxGrenadeCount = AdditiveScaling(8, 4, level);
        }

    }

    [SkillLevelModifier("PlaceMine")]
    class EngiMineSkillModifier : BaseSkillModifer {
        public override int MaxLevel {
            get { return 5; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() { 
                typeof(FireMines),
                typeof(MineArmingFull),
                typeof(MineArmingWeak),
            };
        }

        public override void OnSkillLeveledUp(int level) {
            base.OnSkillLeveledUp(level);
            MineArmingWeak.duration = AdditiveScaling(3, -0.5f, level);
            Logger.Debug("MineArmingWeak.duration: {0}", MineArmingWeak.duration);
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            if(skillState is FireMines) {
                this.OnFireMinesEnter((FireMines)skillState, level);
            } else if(skillState is BaseMineArmingState) {
                this.OnMinesArmedEnter((BaseMineArmingState)skillState, level);
            }
        }

        public void OnFireMinesEnter(FireMines skillState, int level) {

        }

        public void OnMinesArmedEnter(BaseMineArmingState skillState, int level) {
            Logger.Debug("triggerRadius: {0}, blastRadiusScale: {1}, force: {2}, damageScale: {3}", skillState.triggerRadius, skillState.blastRadiusScale, skillState.forceScale, skillState.damageScale);

            if(skillState is MineArmingFull) {
                skillState.damageScale = MultScaling(3, 0.25f, level);
                skillState.blastRadiusScale = MultScaling(2, 0.25f, level);
                skillState.damageScale = MultScaling(3, 0.25f, level);
                skillState.triggerRadius = MultScaling(8, 0.25f, level);

                Transform strongIndicator = skillState.outer.commonComponents.transform.Find(skillState.pathToChildToEnable);
                if(strongIndicator.gameObject.TryGetComponentInChildren(out ObjectScaleCurve objectScaleCurve)) {
                    Vector3 scale = Vector3.one * 2 * skillState.triggerRadius;
                    objectScaleCurve.gameObject.transform.localScale = scale;
                    objectScaleCurve.baseScale = scale;
                    Logger.Debug("objectScaleCurve.baseScale: {0}", objectScaleCurve.baseScale);
                }
            } else if(skillState is MineArmingWeak) {
                skillState.forceScale = MultScaling(1, 0.25f, level);
                skillState.damageScale = MultScaling(1, 0.25f, level);
                skillState.blastRadiusScale = MultScaling(0.2f, 0.25f, level);
                skillState.damageScale = MultScaling(1, 0.25f, level);
                skillState.triggerRadius = MultScaling(4, 0.25f, level);
            }
            Logger.Debug("triggerRadius: {0}, blastRadiusScale: {1}, force: {2}, damageScale: {3}", skillState.triggerRadius, skillState.blastRadiusScale, skillState.forceScale, skillState.damageScale);
        }
    }



    [SkillLevelModifier("PlaceSpiderMine")]
    class EngiSpiderMineSkillModifier : TypedBaseSkillModifier<FireSpiderMine> {
        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("PlaceBubbleShield")]
    class EngiBubbleShieldSkillModifier : TypedBaseSkillModifier<FireBubbleShield> {
        public override int MaxLevel {
            get { return 4; }
        }
        public override void OnSkillLeveledUp(int level) {
            base.OnSkillLeveledUp(level);
            Deployed.lifetime = MultScaling(20, 0.25f, level);
        }

        protected override void OnSkillEnter(FireBubbleShield skillState, int level) {
            base.OnSkillEnter(skillState, level);
            float bubbleSize = MultScaling(20, 0.25f, level);
            // float shieldDuration = MultScaling();

            if(skillState.projectilePrefab.TryGetComponent(out ChildLocator childLocator)) {
                GameObject bubbleGameObject = childLocator.FindChild(Deployed.childLocatorString).gameObject;
                if(bubbleGameObject) {
                    bubbleGameObject.transform.localScale = Vector3.one * bubbleSize;
                }
                
            }
        }
    }

    [SkillLevelModifier("EngiHarpoons")]
    class EngiHarpoonsSkillModifier : TypedBaseSkillModifier<EntityStates.Engi.EngiMissilePainter.Paint> {
        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("PlaceTurret")]
    class EngiTurretSkillModifier : BaseSkillModifer {
        public override int MaxLevel {
            get { return 5; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() {
                typeof(PlaceTurret),
                typeof(FireGauss)
            };
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is PlaceTurret) {

            } else if(skillState is FireGauss) {
                OnFireGaussEnter((FireGauss)skillState, level);
            }
        }

        private void OnFireGaussEnter(FireGauss skillState, int level) {
            // skillState.bullea
            Logger.Debug("damageCoefficient: {0}, bulletCount: {1}", FireGauss.damageCoefficient, FireGauss.bulletCount);
            if(skillState.outer.commonComponents.characterBody.master.TryGetComponent(out BaseAI baseAI)) {
                AISkillDriver skillDriver = baseAI.skillDrivers.FirstOrDefault(it => { return it.customName == "FireAtEnemy"; });
                if(skillDriver != null) {
                    skillDriver.maxDistance = MultScaling(60, 0.25f, level);
                }
            }
        }

        public override void OnSkillLeveledUp(int level) {
            base.OnSkillLeveledUp(level);
            // an extra turret every two levels
            SkillDef.baseMaxStock = (int)AdditiveScaling(2, 0.5f, level);

            FireGauss.damageCoefficient = MultScaling(0.7f, 0.1f, level);
        }

    }

    [SkillLevelModifier("PlaceWalkerTurret")]
    class EngiWalkerTurretSkillModifier : TypedBaseSkillModifier<PlaceWalkerTurret> {
        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level) {
            base.OnSkillLeveledUp(level);
            // Logger.Debug("damageCoefficient: {0}", PlaceWalkerTurret.);

        }
    }
}
