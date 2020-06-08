using System;
using System.Collections.Generic;

using RoR2;
using RoR2.Skills;

using EntityStates;
using EntityStates.Merc;
using EntityStates.Commando.CommandoWeapon;
using R2API.Utils;
using UnityEngine;
using RoR2.Projectile;
using EntityStates.Engi.EngiMissilePainter;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("GroundLight")]
    class SwordSkillModifier : TypedBaseSkillModifier<GroundLight> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            GroundLight.forceMagnitude = MultScaling(600, 0.25f, level);
            GroundLight.selfForceMagnitude = MultScaling(600, 0.25f, level);
            GroundLight.comboDamageCoefficient = MultScaling(1.3f, 0.2f, level);
            GroundLight.finisherDamageCoefficient= MultScaling(3, 0.4f, level);
        }

    }

    [SkillLevelModifier("Whirlwind")]
    class WhirlwindSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 5; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() {
                typeof(WhirlwindEntry),
                typeof(WhirlwindGround),
                typeof(WhirlwindAir)
            };
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is WhirlwindEntry) {
                WhirlwindEntry whirlwindEntry = (WhirlwindEntry)skillState;
            } else if(skillState is WhirlwindBase) {
                WhirlwindBase whirlwindBase = (WhirlwindBase)skillState;
                whirlwindBase.baseDamageCoefficient = MultScaling(whirlwindBase.baseDamageCoefficient, 0.4f, level);
                whirlwindBase.selfForceMagnitude = MultScaling(whirlwindBase.selfForceMagnitude, 0.4f, level);
                Transform modelTransform = whirlwindBase.outer?.commonComponents.modelLocator?.modelTransform;
                if(modelTransform) {
                    // Merc's model has a WhirlwindGround and WhirlwindAir gameobjects that contain the hit boxes as children for the whirlwind attacks
                    // finding the parenting gameobjects turns out to be a good way to increase the size of the hitboxes.
                    Transform whirlwindHitboxTransform = modelTransform.Find(whirlwindBase.hitboxString);
                    if(whirlwindHitboxTransform) {
                        whirlwindHitboxTransform.localScale = Vector3.one * MultScaling(1, 0.5f, level);
                    }
                }
            }
        }

    }

    [SkillLevelModifier("Uppercut")]
    class UppercutSkillModifier : TypedBaseSkillModifier<Uppercut> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("Dash1")]
    class AssaultSkillModifier : TypedBaseSkillModifier<Assaulter> {

        public override int MaxLevel {
            get { return 4; }
        }

        public override void OnSkillExit(Assaulter skillState, int level) {
            base.OnSkillExit(skillState, level);

            HealthComponent characterHealthComponent = skillState.outer?.commonComponents.characterBody?.healthComponent;
            if(characterHealthComponent) {
                float barrierAmount = characterHealthComponent.fullCombinedHealth * AdditiveScaling(0, 0.04f, level); // +5% per level
                if(barrierAmount > 0) {
                    skillState.outer?.commonComponents.characterBody?.healthComponent?.AddBarrier(barrierAmount);
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            Assaulter.damageCoefficient = MultScaling(3, 0.3f, level);

            if(skillDef is MercDashSkillDef) {
                MercDashSkillDef mercDashSkillDef = (MercDashSkillDef)skillDef;
                mercDashSkillDef.timeoutDuration = AdditiveScaling(3, 0.5f, level);
            }

        }
    }

    // both Mercenary special skills have the same skill name
    [SkillLevelModifier("Evis", "Massacre", "Gale-Force")]
    class EviscerateSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 4; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>() {
                typeof(Evis),
                typeof(ThrowEvisProjectile)
            };
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);
            if(skillState is Evis) {
                this.OnEvisEnter((Evis)skillState, level);
            } else if (skillState is ThrowEvisProjectile) {
                this.OnThrowEvisProjectileEnter((ThrowEvisProjectile)skillState, level);
            }
        }

        private void OnEvisEnter(Evis evis, int level) {
            // evis.
        }

        private void OnThrowEvisProjectileEnter(ThrowEvisProjectile throwEvisProjectile, int level) {
            if(throwEvisProjectile.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion)) {
                if(projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileOverlapAttack projectileOverlapAttack)) {
                    float fireFrequency = MultScaling(8f, 0.25f, level);
                    projectileOverlapAttack.damageCoefficient = MultScaling(1, 0.25f, level);
                    projectileOverlapAttack.fireFrequency = fireFrequency;
                    projectileOverlapAttack.resetInterval = 1f / fireFrequency;
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if(skillDef.activationState.stateType == typeof(Evis)) {
                Evis.maxRadius = MultScaling(16, 0.5f, level);
                Evis.damageFrequency = MultScaling(7, 0.20f, level);
                Evis.procCoefficient = MultScaling(1, 0.1f, level);
            } else if (skillDef.activationState.stateType == typeof(ThrowEvisProjectile)){

            }
        }

    }
}
