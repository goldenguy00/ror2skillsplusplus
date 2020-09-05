using System;
using System.Collections.Generic;
using EntityStates;

using RoR2;
using RoR2.Skills;
using R2API.Utils;

using UnityEngine;
using Rewired;

using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.Util;
using SkillsPlusPlus.ConVars;

namespace SkillsPlusPlus {

    [RequireComponent(typeof(PlayerCharacterMasterController))]
    sealed class SkillPointsController : MonoBehaviour {

        private const int SKILL_DISABLED = -1;

        private PlayerCharacterMasterController playerCharacterMasterController;
        private CharacterBody body {
            get { return playerCharacterMasterController.master.GetBody(); }
        }
        private SkillLocator skillLocator {
            get { return body?.skillLocator; }
        }

        private TeamIndex PlayerTeamIndex {
            get {
                if (playerCharacterMasterController.master.hasBody) {
                    return playerCharacterMasterController.master.GetBody().teamComponent.teamIndex;
                }
                return TeamIndex.None;
            }
        }

        //  private Dictionary<SkillSlot, int> spentSkillPoints;
        private Dictionary<string, int> skillLevels;

        private int earnedSkillPoints = 0;
        private int unspentSkillPoints = 0;
        private int levelsPerSkillPoint = 5;

        private bool isSurvivorEnabled = true;

        void Awake() {
            this.levelsPerSkillPoint = ConVars.ConVars.levelsPerSkillPoint.value;
            // this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillLevels = new Dictionary<string, int>();

            this.playerCharacterMasterController = this.GetComponent<PlayerCharacterMasterController>();

            this.playerCharacterMasterController.master.onBodyStart += OnBodyStart;
            if(this.playerCharacterMasterController.master.hasBody) {
                this.OnBodyStart(this.playerCharacterMasterController.master.GetBody());
            }
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter += OnEnterState;
            On.EntityStates.EntityState.OnExit += OnExitState;
            On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateState;
            On.EntityStates.GenericCharacterMain.CanExecuteSkill += this.GenericCharacterMain_CanExecuteSkill;
        }

        void OnDestroy() {
            this.playerCharacterMasterController.master.onBodyStart -= OnBodyStart;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit -= GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter -= OnEnterState;
            On.EntityStates.EntityState.OnExit -= OnExitState;
            On.RoR2.CharacterBody.RecalculateStats -= this.OnRecalculateState;

        }

        #region Hooks

        private bool GenericCharacterMain_CanExecuteSkill(On.EntityStates.GenericCharacterMain.orig_CanExecuteSkill orig, GenericCharacterMain self, GenericSkill skillSlot) {
            if(this.isSurvivorEnabled == true && this.body != null && self.outer.commonComponents.characterBody == this.body) {
                Player inputPlayer = this.playerCharacterMasterController?.networkUser?.localUser?.inputPlayer;
                if(inputPlayer != null && inputPlayer.GetButtonDown(SkillInput.BUY_SKILLS_ACTION_ID)) {
                    return false;
                }
            }
            return orig(self, skillSlot);
        }
        private void OnRecalculateState(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self == this.body) {
                this.OnLevelChanged();
            }
        }

        private bool isInitialised = false;
        private void OnBodyStart(CharacterBody body) {
            Logger.Debug("OnBodyStart({0})", body);
#if DEBUG
            if(body.healthComponent.godMode == false) {
                body.healthComponent.godMode = true;
            }
#endif

            if(isInitialised) {
                return;
            }

            if(body != null) {
                this.isSurvivorEnabled = ConVars.ConVars.disabledSurvivors.value.Contains(body.GetDisplayName()) == false;
                if(this.isSurvivorEnabled == false) {
                    isInitialised = true;
                    Logger.Warn("Skills++ has been disable for this survivor. (survivorName = {0})", body.GetDisplayName());
                    return;
                }
            }

            var skillLocator = body.skillLocator;
            if(skillLocator == null) {
                Logger.Debug("Unable to initialise skill modifiers since there is no skill locator to use");
                return;
            }

            isInitialised = true;
            foreach(GenericSkill genericSkill in skillLocator.FindAllGenericSkills()) {
                if(genericSkill == null) {
                    continue;
                }
                BaseSkillModifier modifier = GetModifierForGenericSkill(genericSkill);
                if(modifier == null) {
                    continue;
                }

                SetSkillLevelForGenericSkill(genericSkill, 0);
                int skillLevel = GetSkillLevelForGenericSkill(genericSkill);

                SkillDef baseSkillDef = Instantiate(genericSkill.baseSkill);
                genericSkill.onSkillChanged -= this.OnSkillChanged;
                genericSkill.onSkillChanged += this.OnSkillChanged;

                modifier.OnSkillLeveledUp(skillLevel, this.body, baseSkillDef);
                genericSkill.SetBaseSkill(baseSkillDef);

                // #22
                // This mod is instantiating a clone of the generic skills current skill definition
                // This caused a bug for Acrid where its passive would not trigger due to the cloned skill def not being 
                // equal to other preset SkillDefs.
                // Here we access Acrid's damage controller and assign the skill definitions to uphold the equality 
                if(body.TryGetComponent(out CrocoDamageTypeController crocoDamageTypeController)) {
                    if(genericSkill.skillDef.skillName == crocoDamageTypeController.poisonSkillDef.skillName) {
                        crocoDamageTypeController.poisonSkillDef = baseSkillDef;
                    }
                    if(genericSkill.skillDef.skillName == crocoDamageTypeController.blightSkillDef.skillName) {
                        crocoDamageTypeController.blightSkillDef = baseSkillDef;
                    }
                }
            }

        }

        private int GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot) {
            if(this.isSurvivorEnabled == false) {
                return orig(self, slot);
            }

            int bonusSlots = 0;
            if(self == this.playerCharacterMasterController.master) {
                bonusSlots = EngiSkillModifier.GetDeployableSameSlotBonus(slot);
            }
            return orig(self, slot) + bonusSlots;
        }

        private void OnEnterState(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            if(isSurvivorEnabled) {
                if(TryGetSkillModifierForState(self, out BaseSkillModifier skillModifier, out int skillLevel)) {
                    skillModifier.OnSkillEnter(self, skillLevel);
                }
            }
            orig(self);
        }

        private void OnExitState(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if(isSurvivorEnabled && self is BaseState baseState) {
                if(TryGetSkillModifierForState(baseState, out BaseSkillModifier skillModifier, out int skillLevel)) {
                    skillModifier.OnSkillExit(baseState, skillLevel);                    
                }
            }
            orig(self);
        }

        #endregion

        private bool TryGetSkillModifierForState(BaseState state, out BaseSkillModifier skillModifierOut, out int skillLevelOut) {
            var entityStateMachine = state.outer;
            if(entityStateMachine != null && entityStateMachine.destroying == false) {
                bool belongsToCharacter = false;
                if(this.body != null && entityStateMachine.commonComponents.characterBody == this.body) {
                    belongsToCharacter = true;
                } else if(entityStateMachine.commonComponents.projectileController != null && entityStateMachine.commonComponents.projectileController.owner == this.body.gameObject) {
                    belongsToCharacter = true;
                } else if(this.playerCharacterMasterController.master != null && entityStateMachine.commonComponents.characterBody?.master?.minionOwnership?.ownerMaster == this.playerCharacterMasterController.master) {
                    belongsToCharacter = true;
                } else if(this.body != null && state.outer.TryGetComponent(out GenericOwnership ownership) && ownership.ownerObject == this.body.gameObject) {
                    belongsToCharacter = true;
                }

                if(belongsToCharacter == false) {
                    skillModifierOut = null;
                    skillLevelOut = SKILL_DISABLED;
                    return false;
                }

                var stateType = state.GetType();
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(stateType);
                if(skillModifier != null) {
                    foreach(var skillName in skillModifier.skillNames) {
                        var genericSkill = this.body.skillLocator?.FindGenericSkill(skillName);

                        skillLevelOut = this.GetSkillLevelForGenericSkill(genericSkill);
                        if(skillLevelOut != SKILL_DISABLED) {
                            skillModifierOut = skillModifier;
                            return true;
                        }
                    }
                }
            } else {
                Logger.Debug("{0} does not have a running entity state machine", state);            
            }
            skillLevelOut = SKILL_DISABLED;
            skillModifierOut = null;
            return false;
        }

        public int GetSkillLevelForGenericSkill(GenericSkill genericSkill) {
            if(genericSkill == null) {
                return SKILL_DISABLED;
            }
            try {
                var skillOverrides = genericSkill.GetFieldValue<GenericSkill.SkillOverride[]>("skillOverrides");
                var skillOverridePriority = GenericSkill.SkillOverridePriority.Default;
                var skillIndex = -1;
                for(int i = 0; i < skillOverrides.Length; i++) {
                    GenericSkill.SkillOverridePriority priority = skillOverrides[i].priority;
                    if(skillOverridePriority <= priority) {
                        skillIndex = i;
                        skillOverridePriority = priority;
                    }
                }
                if(skillOverridePriority >= GenericSkill.SkillOverridePriority.Contextual) {
                    return SKILL_DISABLED;
                }
                //if(skillIndex == -1) {
                //    skillName = genericSkill?.baseSkill?.skillName;
                //}
            } catch {
                // skillName = genericSkill?.baseSkill?.skillName;
            }
            var baseSkillName = genericSkill?.baseSkill?.skillName;
            if(baseSkillName == null) {
                return SKILL_DISABLED;
            }
            if(skillLevels.ContainsKey(baseSkillName)) {
                return skillLevels[baseSkillName];
            }
            return SKILL_DISABLED;
        }

        private void SetSkillLevelForGenericSkill(GenericSkill genericSkill, int newLevel) {
            var baseSkillName = genericSkill?.baseSkill?.skillName;
            if(baseSkillName != null) {
                skillLevels[baseSkillName] = newLevel;
                Logger.Debug("Setting {0} to level {1}", baseSkillName, newLevel);
            }
        }

        public BaseSkillModifier GetModifierForGenericSkill(GenericSkill genericSkill) {
            return SkillModifierManager.GetSkillModifier(genericSkill.skillDef.skillName);
        }

        public bool CanUpgradeGenericSkill(GenericSkill genericSkill) {
            if(genericSkill == null) {
                return false;
            }
            if(GetSkillLevelForGenericSkill(genericSkill) == SKILL_DISABLED) {
                return false;
            }
            if(GetModifierForGenericSkill(genericSkill) == null) {
                return false;
            }
            return unspentSkillPoints > 0;
        }

        private void OnSkillChanged(GenericSkill genericSkill) {
            var skillDef = genericSkill.skillDef;
            var baseSkillDef = genericSkill.baseSkill;
            if(baseSkillDef != null) {
                int skillLevel = GetSkillLevelForGenericSkill(genericSkill);
                BaseSkillModifier modifier = GetModifierForGenericSkill(genericSkill);
                if(modifier != null && skillLevel != SKILL_DISABLED) {
                    // get the skill level by the base skill name but provide the active skill def
                    modifier.OnSkillLeveledUp(skillLevel, this.body, skillDef);
                }
            }
        }

        void Update() {

#if DEBUG
            if (Input.GetKeyDown(KeyCode.Equals) && this.PlayerTeamIndex != TeamIndex.None) {
                //TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(500 * Time.deltaTime));
                //TeamManager.instance.SetTeamLevel(this.PlayerTeamIndex, TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex) + 1);
                Logger.Debug("Giving points");
                this.earnedSkillPoints++;
                this.unspentSkillPoints++;
            }
            if(Input.GetKeyDown(KeyCode.Keypad1) && playerCharacterMasterController != null && body != null) {
                GameObject teleporter = GameObject.Find("Teleporter1(Clone)");
                Transform spawnLocation = body.transform;
                if(teleporter != null && teleporter.TryGetComponent(out TeleporterInteraction teleporterInteraction)) {
                    GameObject.Instantiate(teleporterInteraction.shopPortalSpawnCard.prefab, spawnLocation.position, spawnLocation.rotation, null);
                }
            }
            if(Input.GetKeyDown(KeyCode.Keypad2) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.UtilitySkillMagazine);
            }
            if(Input.GetKeyDown(KeyCode.Keypad3) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.ExtraLife);
            }
            if(Input.GetKeyDown(KeyCode.Keypad4) && this.playerCharacterMasterController != null) {
                this.body.skillLocator.ResetSkills();
            }
            if(Input.GetKeyDown(KeyCode.Keypad5) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("CIScepter");
            }
            if(Input.GetKeyDown(KeyCode.Keypad6) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.Syringe, 5);
            }
            if(Input.GetKeyDown(KeyCode.Keypad7) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.LunarPrimaryReplacement);
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.LunarUtilityReplacement);
            }
#endif

        }

        public void OnBuySkill(GenericSkill genericSkill) {
            if(genericSkill == null) {
                return;
            }
            if (unspentSkillPoints <= 0) {
                return;
            }
            BaseSkillModifier modifier = GetModifierForGenericSkill(genericSkill);
            if(modifier == null) {
                Logger.Debug("Could not purchase skill. Could not find a skill modifiers matching {0}", genericSkill.skillDef.skillName);
                return;
            }

            unspentSkillPoints--;

            int skillLevel = GetSkillLevelForGenericSkill(genericSkill);
            // increment and store the new skill level
            SetSkillLevelForGenericSkill(genericSkill, ++skillLevel);
            Logger.Debug("SkillSlot {0} @ level {1}", genericSkill.baseSkill.skillName, skillLevel);

            // find an notify the modifer to update the skill's parameters
            modifier.OnSkillLeveledUp(skillLevel, this.body, genericSkill.skillDef);
            genericSkill.RecalculateValues();     
        }

        public void OnLevelChanged() {
            int characterLevel = (int) TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex);
            // Logger.Debug("OnLevelChanged({0}) for team {1}", characterLevel, PlayerTeamIndex);
            if (this.PlayerTeamIndex == TeamIndex.None) {
                return;
            }

            int newSkillPoints = Math.Max(0, SkillPointsAtLevel(characterLevel) - earnedSkillPoints);

            earnedSkillPoints += newSkillPoints;
            unspentSkillPoints += newSkillPoints;
        }

        private int SkillPointsAtLevel(int characterLevel) {
#if DEBUG
            return characterLevel - 1;
#else
            if(levelsPerSkillPoint <= 1) {
                return characterLevel - 1;
            }
            return (characterLevel / this.levelsPerSkillPoint);
#endif 
        }
    }
}
