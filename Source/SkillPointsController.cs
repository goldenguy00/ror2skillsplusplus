using System;
using System.Collections.Generic;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;

using SkillsPlusPlus.Modifiers;
using System.Linq;

using SkillsPlusPlus.Util;
using SkillsPlusPlus.ConVars;

using Rewired;

namespace SkillsPlusPlus {

    [RequireComponent(typeof(PlayerCharacterMasterController))]
    sealed class SkillPointsController : MonoBehaviour {

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

        private SkillLevelIconController[] skillIconControllers;

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
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter += OnEnterState;
            On.EntityStates.EntityState.OnExit += OnExitState;
            On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateState;
            On.EntityStates.GenericCharacterMain.CanExecuteSkill += this.GenericCharacterMain_CanExecuteSkill;
        }

        private bool GenericCharacterMain_CanExecuteSkill(On.EntityStates.GenericCharacterMain.orig_CanExecuteSkill orig, GenericCharacterMain self, GenericSkill skillSlot) {
            if(this.isSurvivorEnabled == true && this.body != null && self.outer.commonComponents.characterBody == this.body) {
                Player inputPlayer = this.playerCharacterMasterController?.networkUser?.localUser?.inputPlayer;
                if(inputPlayer != null && inputPlayer.GetButtonDown(SkillInput.BUY_SKILLS_ACTION_ID)) {
                    return false;
                }
            }
            return orig(self, skillSlot);
        }

        void OnDestroy() {
            this.playerCharacterMasterController.master.onBodyStart -= OnBodyStart;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit -= GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter -= OnEnterState;
            On.EntityStates.EntityState.OnExit -= OnExitState;
            On.RoR2.CharacterBody.RecalculateStats -= this.OnRecalculateState;

        }
        private void OnRecalculateState(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self == this.body) {
                this.OnLevelChanged();
            }
        }

        private void OnBodyStart(CharacterBody body) {
            EnsureSkillModifiersAreInitialised(true);            
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

        private bool TryGetSkillModifierForState(BaseState state, out ISkillModifier skillModifierOut, out string skillNameOut) {
            var entityStateMachine = state.outer;
            if(entityStateMachine != null && entityStateMachine.destroying == false) {
                // if(entityStateMachine.networker != null && entityStateMachine.networker.localPlayerAuthority) {
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
                    // Logger.Debug("Could not associate {0} to the current player", state);
                    skillModifierOut = null;
                    skillNameOut = null;
                    return false;
                }

                var stateType = state.GetType();
                IEnumerable<ISkillModifier> skillModifiers = SkillModifierManager.GetSkillModifiersForEntityStateType(stateType);
                this.EnsureSkillModifiersAreInitialised();
                foreach(ISkillModifier skillModifier in skillModifiers) {
                    var skillName = skillModifier?.skillName;
                    if(skillName == null) {
                        Logger.Debug("Skill modifier {0} does not have a skill name", skillModifier);
                        continue;
                    }

                    var genericSkill = this.skillLocator.FindGenericSkill(skillName, true);
                    if(genericSkill == null) {
                        Logger.Debug("Could not find generic skill instance for skill named {0}", skillName);
                        continue;
                    }
                    // Logger.Debug("Intercepted entity state {1} for skill named {0}", skillModifier.SkillDef.skillName, stateType.Name);
                    skillNameOut = skillName;
                    skillModifierOut = skillModifier;
                    return true;
                }

                // } else {
                //     Logger.Debug("{0} does not belong to a entity state machine");
                // }
            } else {
                Logger.Debug("{0} does not have a running entity state machine", state);            
            }
            skillNameOut = null;
            skillModifierOut = null;
            return false;
        }

        private void OnEnterState(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            if(isSurvivorEnabled && TryGetSkillModifierForState(self, out ISkillModifier skillModifier, out string skillName) && this.skillLevels.TryGetValue(skillName, out int level)) {
                skillModifier.OnSkillEnter(self, level);
            }
            orig(self);
        }

        private void OnExitState(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if (isSurvivorEnabled && self is BaseState baseState) {
                if (TryGetSkillModifierForState(baseState, out ISkillModifier skillModifier, out string skillName)) {
                    if(this.skillLevels.TryGetValue(skillName, out int level)) {
                        skillModifier.OnSkillExit(baseState, level);
                    }
                }
            }
            orig(self);
        }

        private bool skillModifiersAreInitialised = false;
        private void EnsureSkillModifiersAreInitialised(bool force = false) {

            if (skillModifiersAreInitialised && !force) {
                return;
            }

            if(body != null) {
                this.isSurvivorEnabled = ConVars.ConVars.disabledSurvivors.value.Contains(body.GetDisplayName()) == false;
                if(this.isSurvivorEnabled == false) {
                    skillModifiersAreInitialised = true;
                    Logger.Warn("Skills++ has been disable for this survivor. (survivorName = {0})", body.GetDisplayName());
                    return;
                }
            }
            

            var skillLocator = this.skillLocator;
            if (skillLocator == null) {
                Logger.Debug("Unable to initialise skill modifiers since there is no skill locator to use");
                return;
            }
            foreach(GenericSkill genericSkill in skillLocator.FindAllGenericSkills()) {
                if(skillLevels.ContainsKey(genericSkill.baseSkill.skillName) == false) {
                    Logger.Debug("Setting {0} to level 1", genericSkill.skillDef.skillName);
                    skillLevels[genericSkill.skillDef.skillName] = 0;
                }
            }

            skillModifiersAreInitialised = true;
            foreach (GenericSkill genericSkill in skillLocator.FindAllGenericSkills()) {
                if (genericSkill == null) {
                    continue;
                }
                SkillDef baseSkillDef = Instantiate(genericSkill.baseSkill);
                BaseSkillModifier modifier = SkillModifierManager.GetSkillModifier(baseSkillDef.skillName);
                genericSkill.onSkillChanged -= this.OnSkillChanged;
                genericSkill.onSkillChanged += this.OnSkillChanged;
                if(modifier != null) {
                    // modifier.SkillDef = baseSkillDef;
                    modifier.skillName = baseSkillDef.skillName;
                    modifier.OnSkillLeveledUp(skillLevels[baseSkillDef.skillName], this.body, baseSkillDef);
                    genericSkill.SetBaseSkill(baseSkillDef);
                }
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

        private void OnSkillChanged(GenericSkill genericSkill) {
            Logger.Debug("OnSkillChanged({0})", genericSkill);
            var skillDef = genericSkill.skillDef;
            var baseSkillDef = genericSkill.baseSkill;
            if(baseSkillDef == null) {
                return;
            }
            Logger.Debug("OnSkillChanged({0}:{1})", genericSkill, skillDef.skillName);
            BaseSkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef.skillName);
            // get the skill level by the base skill name but provide the active skill def
            modifier.OnSkillLeveledUp(skillLevels[baseSkillDef.skillName], this.body, skillDef);
        }

        public void SetSkillIconControllers(SkillLevelIconController[] skillIconControllers) {
            if (this.skillIconControllers != null) {
                foreach (SkillLevelIconController skillIconController in this.skillIconControllers) {
                    skillIconController.OnUpgradeSkill -= this.OnBuySkill;
                }
            }
            this.skillIconControllers = skillIconControllers;
            if (skillIconControllers != null) {
                foreach (SkillLevelIconController skillIconController in skillIconControllers) {
                    skillIconController.OnUpgradeSkill += this.OnBuySkill;
                }
                RefreshIconControllers();
            }
        }

        void Update() {

            EnsureSkillModifiersAreInitialised();

            if(this.body == null && this.skillIconControllers != null) {
                // #14
                // if the body is null then we must unset the skillIconControllers.
                // it it likely that the character has just left the scene and the hud is still present
                this.skillIconControllers = null;
            }

#if DEBUG
            if (Input.GetKeyDown(KeyCode.Equals) && this.PlayerTeamIndex != TeamIndex.None) {
                //TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(500 * Time.deltaTime));
                //TeamManager.instance.SetTeamLevel(this.PlayerTeamIndex, TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex) + 1);
                this.earnedSkillPoints++;
                this.unspentSkillPoints++;
                RefreshIconControllers();
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

        private void OnBuySkill(string skillName) {
            if(skillName == null) {
                return;
            }
            if (unspentSkillPoints <= 0) {
                return;
            }

            string resolvedSkillName = ResolveSkillNameToInternalName(skillName);
            Logger.Debug("OnBuySkill({0}), resolvedName: {1}", skillName, resolvedSkillName);

            if (skillLevels.TryGetValue(resolvedSkillName, out int skillLevel)) {
                GenericSkill genericSkill = skillLocator.FindGenericSkill(resolvedSkillName, true);
                ISkillModifier modifier = SkillModifierManager.GetSkillModifier(genericSkill.skillDef.skillName);
                if(genericSkill == null) {
                    Logger.Debug("Could not purchase skill. Could not find generic skill matching {0} or {1}", skillName, resolvedSkillName);
                    return;
                }
                if(modifier == null || modifier is NoopSkillModifier) {
                    Logger.Debug("Could not purchase skill. Could not find a skill modifiers matching {0}", skillName);
                    return;
                }

                unspentSkillPoints--;

                // increment and store the new skill level
                skillLevels[resolvedSkillName] = ++skillLevel;
                Logger.Debug("SkillSlot {2} ({0}) @ level {1}", skillName, skillLevel, resolvedSkillName);

                // find an notify the modifer to update the skill's parameters
                if (modifier != null) {
                    modifier.OnSkillLeveledUp(skillLevel, this.body, genericSkill.skillDef);
                    genericSkill.RecalculateValues();
                }
                RefreshIconControllers();
            }
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

            RefreshIconControllers();
        }

        private void RefreshIconControllers() {
            var skillLocator = this.skillLocator;
            if(skillIconControllers != null && this.PlayerTeamIndex != TeamIndex.None && skillLocator != null) {
                int characterLevel = (int) TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex);
                foreach (SkillLevelIconController skillLevelIconController in skillIconControllers) {
                    string skillName = ResolveSkillNameToInternalName(skillLevelIconController.skillName);
                    if(skillName != null && skillLevels.TryGetValue(skillName, out int currentSkillLevel)) {
                        // Logger.Debug("RefreshIconControllers - slot: {0}, skillLevelIconController: {1}, modifier: {2}", slot, skillLevelIconController, modifier);

                        // has skillpoints to spend
                        // and the skill is less than its max level
                        skillLevelIconController.IsUpgradable = unspentSkillPoints > 0 && isSurvivorEnabled;
                        skillLevelIconController.Level = currentSkillLevel;
                    } else {
                        Logger.Debug("Could not refresh the icon controller for skill named {0}", skillName);
                    }
                }
            }
        }

        private string ResolveSkillNameToInternalName(string skillName) {
            var genericSkill = this.skillLocator.FindGenericSkill(skillName);
            if(genericSkill == null) {
                return skillName;
            }
            return genericSkill.baseSkill.skillName;      
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
