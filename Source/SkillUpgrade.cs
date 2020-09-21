using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using EntityStates;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus {

    [RequireComponent(typeof(CharacterBody))]
    class SkillUpgrade : NetworkBehaviour {

        [SyncVar(hook = "OnSkillLevelChanged")]
        public int skillLevel;

        public SkillPointsController skillPointsController;
        public GenericSkill targetGenericSkill;

        public string targetBaseSkillName;

        CharacterBody characterBody;

        void Awake() {

            Logger.Debug("Awake()");
            this.characterBody = this.GetComponent<CharacterBody>();
            this.targetGenericSkill.onSkillChanged += OnSkillChanged;
            this.targetBaseSkillName = targetGenericSkill.baseSkill.skillName;
            if (this.characterBody.masterObject) {
                Logger.Warn("Character has a master");
                if (this.characterBody.master.GetBody() == this.characterBody) {
                    Logger.Warn("And it already matches this current character body reference");
                } else {
                    Logger.Warn("And it doesn't match this current character body reference!!!!!!!!!!");
                }
            } else {
                Logger.Warn("Character has no master");
            }
            // On.EntityStates.BaseState.OnEnter += OnBaseStateEnter;
            // On.EntityStates.EntityState.OnExit += OnBaseStateExit;
        }

        void Update() {
            this.skillPointsController = characterBody.masterObject?.GetComponent<SkillPointsController>();
        }

        void OnDestroy() {
            if (targetGenericSkill) {
                targetGenericSkill.onSkillChanged -= this.OnSkillChanged;
            }
            if (this.skillPointsController) {
                this.skillPointsController.PersistUpgrade(skillLevel, this.targetBaseSkillName);
            }
        }

        void OnSkillChanged(GenericSkill genericSkill) {
            RefreshUpgrades();
        }

        void OnSkillLevelChanged(int newSkillLevel) {
            RefreshUpgrades();
        }

        void RefreshUpgrades() {
            var activeSkillDef = GetActiveSkillDef(targetGenericSkill);
            if (activeSkillDef == null) {
                return;
            }
            var modifier = SkillModifierManager.GetSkillModifiersForEntityStateType(activeSkillDef.activationState.stateType);
            if (modifier != null) {
                // TODO: rename OnSkillLeveledUp to OnSkillChanged
                modifier.OnSkillLeveledUp(this.skillLevel, this.characterBody, activeSkillDef);
            }
        }

        [Server]
        public void OnBuySkill() {
            if (skillPointsController && this.CanUpgradeSkill()) {
                skillPointsController.DeductSkillPoints(1);
                this.skillLevel += 1;
                RefreshUpgrades();
            }
        }

        public bool CanUpgradeSkill() {
            if (targetGenericSkill == null) {
                return false;
            }
            if (skillPointsController && !skillPointsController.hasUnspentPoints) {
                return false;
            }
            return SkillModifierManager.HasSkillModifier(this.targetBaseSkillName);
        }

        private CharacterBody FindOwningCharacterBody(EntityState state) {
            if (state.outer) {
                if (state.outer.TryGetComponent(out CharacterBody characterBody)) {
                    return characterBody;
                } else if (state.outer.TryGetComponent(out ProjectileController projectileController) && projectileController.owner) {
                    return projectileController.owner.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out MinionOwnership minionOwnership) && minionOwnership.ownerMaster) {
                    return minionOwnership.ownerMaster.GetBody();
                } else if (state.outer.TryGetComponent(out GenericOwnership genericOwnership) && genericOwnership.ownerObject) {
                    return genericOwnership.ownerObject.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out Deployable deployable) && deployable.ownerMaster) {
                    return deployable.ownerMaster.GetBody();
                }
            }
            return null;
        }

        private static SkillDef GetActiveSkillDef(GenericSkill genericSkill) {
            if (genericSkill == null) {
                return null;
            }
            try {
                var skillOverrides = genericSkill.GetFieldValue<GenericSkill.SkillOverride[]>("skillOverrides");
                var skillOverridePriority = GenericSkill.SkillOverridePriority.Default;
                var skillIndex = -1;
                for (int i = 0; i < skillOverrides.Length; i++) {
                    GenericSkill.SkillOverridePriority priority = skillOverrides[i].priority;
                    if (skillOverridePriority <= priority) {
                        skillIndex = i;
                        skillOverridePriority = priority;
                    }
                }
                // the currently active skill in the generic skill is only temporary
                // so there is no upgrade context for it
                if (skillOverridePriority >= GenericSkill.SkillOverridePriority.Contextual) {
                    return null;
                } else if (skillIndex != -1) {
                    return skillOverrides[skillIndex].skillDef;
                } else {
                    return genericSkill.baseSkill;
                }
            } catch {
                return genericSkill.baseSkill;
            }
        }

        private void OnBaseStateEnter(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            if (FindOwningCharacterBody(self)?.gameObject == this.gameObject && self is BaseState baseState) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                if (skillModifier != null) {
                    skillModifier.OnSkillEnter(skillState: baseState, this.skillLevel);
                    orig(self);
                    return;
                }
            }
            orig(self);

        }

        private void OnBaseStateExit(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if (FindOwningCharacterBody(self)?.gameObject == this.gameObject && self is BaseState baseState) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                if (skillModifier != null) {
                    skillModifier.OnSkillExit(skillState: baseState, this.skillLevel);
                    orig(self);
                    return;
                }
            } else {
                orig(self);
            }
        }

    }
}