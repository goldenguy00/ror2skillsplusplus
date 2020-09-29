using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private bool isSurvivorEnabled {
            get { return skillPointsController?.isSurvivorEnabled == true; }
        }

        void Awake() {
            this.characterBody = this.GetComponent<CharacterBody>();
            this.targetBaseSkillName = targetGenericSkill.baseSkill.skillName;
        }

        void OnEnable() {
            this.targetGenericSkill.onSkillChanged += OnSkillChanged;
            On.EntityStates.BaseState.OnEnter += this.OnBaseStateEnter;
            On.EntityStates.EntityState.OnExit += this.OnBaseStateExit;
            RefreshUpgrades();
        }

        void OnDisable() {
            On.EntityStates.BaseState.OnEnter -= this.OnBaseStateEnter;
            On.EntityStates.EntityState.OnExit -= this.OnBaseStateExit;
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

        [Client]
        void OnSkillLevelChanged(int newSkillLevel) {
            Logger.Debug("OnSkillLevelChanged({0})", newSkillLevel);
            this.skillLevel = newSkillLevel;
            RefreshUpgrades();
        }

        void RefreshUpgrades() {
            if (!isSurvivorEnabled) return;
            var activeSkillDef = GetActiveSkillDef(targetGenericSkill);
            if (activeSkillDef == null) {
                return;
            }
            var modifier = SkillModifierManager.GetSkillModifier(activeSkillDef);
            if (modifier != null) {
                // TODO: rename OnSkillLeveledUp to OnSkillChanged
                modifier.OnSkillLeveledUp(this.skillLevel, this.characterBody, activeSkillDef);
                if (targetGenericSkill) {
                    targetGenericSkill.RecalculateValues();
                }
            }
        }

        public void OnBuySkill() {
            CmdOnBuySkill(this.targetBaseSkillName);
        }

        [Command]
        [Server]
        private void CmdOnBuySkill(string targetSkillName) {
            if (!isSurvivorEnabled) return;
            var allSkillUpgrades = this.GetComponents<SkillUpgrade>();
            foreach (var skillUpgrade in allSkillUpgrades) {
                if (skillUpgrade.targetBaseSkillName == targetSkillName) {
                    if (skillPointsController && skillUpgrade.CanUpgradeSkill()) {
                        skillPointsController.DeductSkillPoints(1);
                        skillUpgrade.skillLevel += 1;
                        Logger.Debug("CmdOnBuySkill({0}): skillLevel: {1}", skillUpgrade.targetBaseSkillName, skillUpgrade.skillLevel);
                        skillUpgrade.RefreshUpgrades();
                    }
                }
            }
        }

        public bool CanUpgradeSkill() {
            if (!isSurvivorEnabled) return false;
            if (targetGenericSkill == null) return false;
            if (skillPointsController && !skillPointsController.hasUnspentPoints) return false;
            return SkillModifierManager.HasSkillModifier(this.targetBaseSkillName);
        }

        private CharacterBody FindOwningCharacterBody(EntityState state) {
            if (state == null) {
                return null;
            }
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
            if (isSurvivorEnabled && FindOwningCharacterBody(self)?.gameObject == this.gameObject && self is BaseState baseState) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                var activeSkillDef = GetActiveSkillDef(this.targetGenericSkill);
                if (skillModifier != null && activeSkillDef != null && skillModifier.skillNames.Contains(activeSkillDef.skillName)) {
                    skillModifier.OnSkillEnter(skillState: baseState, this.skillLevel);
                    orig(self);
                    return;
                }
            }
            orig(self);

        }

        private void OnBaseStateExit(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if (isSurvivorEnabled && self is BaseState baseState && FindOwningCharacterBody(self)?.gameObject == this.gameObject) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                var activeSkillDef = GetActiveSkillDef(this.targetGenericSkill);
                if (skillModifier != null && activeSkillDef != null && skillModifier.skillNames.Contains(activeSkillDef.skillName)) {
                    skillModifier.OnSkillExit(skillState: baseState, this.skillLevel);
                    orig(self);
                    return;
                }
            }
            orig(self);
        }

    }
}