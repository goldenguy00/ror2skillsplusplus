using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EntityStates;
using R2API.Utils;
using Rewired;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus {

    [RequireComponent(typeof(PlayerCharacterMasterController))]
    [RequireComponent(typeof(NetworkIdentity))]
    sealed class SkillPointsController : NetworkBehaviour {

        // private const int SKILL_DISABLED = -1;

        private PlayerCharacterMasterController playerCharacterMasterController;
        private NetworkIdentity networkIdentity;

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

        private SyncListSkillUpgrade skillUpgrades = new SyncListSkillUpgrade();

        [SyncVar]
        private int earnedSkillPoints = 0;

        [SyncVar]
        private int unspentSkillPoints = 0;

        [SyncVar]
        private int levelsPerSkillPoint = 5;

        private bool isSurvivorEnabled = true;

        public class SyncListSkillUpgrade : SyncListStruct<SkillUpgrade> { }
        public struct SkillUpgrade {
            public string skillName;
            public int skillLevel;
        }

        void Awake() {
            // this.spentSkillPoints = new Dictionary<SkillSlot, int>();
            this.skillUpgrades.Callback += (operation, index) => {
                Logger.Debug("skillUpgrades did change");
                foreach (var skillUpgrade in this.skillUpgrades) {
                    Logger.Debug("{0}:{1}", skillUpgrade.skillName, skillUpgrade.skillLevel);
                    var genericSkill = this.skillLocator?.FindGenericSkillBySkillDef(skillUpgrade.skillName, true);
                    OnSkillChanged(genericSkill);
                }
            };

            this.playerCharacterMasterController = this.GetComponent<PlayerCharacterMasterController>();
            this.networkIdentity = this.GetComponent<NetworkIdentity>();

            this.playerCharacterMasterController.master.onBodyStart += OnBodyStart;
            if (this.playerCharacterMasterController.master.hasBody) {
                this.OnBodyStart(this.playerCharacterMasterController.master.GetBody());
            }
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter += OnEnterState;
            On.EntityStates.EntityState.OnExit += OnExitState;
            On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateStats;
            On.EntityStates.GenericCharacterMain.CanExecuteSkill += this.GenericCharacterMain_CanExecuteSkill;
        }

        [Server]
        void Start() {
            this.levelsPerSkillPoint = ConVars.ConVars.levelsPerSkillPoint.value;
        }

        void OnDestroy() {
            this.playerCharacterMasterController.master.onBodyStart -= OnBodyStart;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit -= GetDeployableSameSlotLimit;
            On.EntityStates.BaseState.OnEnter -= OnEnterState;
            On.EntityStates.EntityState.OnExit -= OnExitState;
            On.RoR2.CharacterBody.RecalculateStats -= this.OnRecalculateStats;
        }

        #region Hooks

        private bool GenericCharacterMain_CanExecuteSkill(On.EntityStates.GenericCharacterMain.orig_CanExecuteSkill orig, GenericCharacterMain self, GenericSkill skillSlot) {
            if (this.isSurvivorEnabled == true && this.body != null && self.outer.commonComponents.characterBody == this.body) {
                Player inputPlayer = this.playerCharacterMasterController?.networkUser?.localUser?.inputPlayer;
                if (inputPlayer != null && inputPlayer.GetButtonDown(SkillInput.BUY_SKILLS_ACTION_ID)) {
                    return false;
                }
            }
            return orig(self, skillSlot);
        }
        private void OnRecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self == this.body && this.isServer) {
                this.OnLevelChanged();
            }
        }

        //private bool isInitialised = false;
        [Server]
        private void OnBodyStart(CharacterBody body) {
            if (body == null) {
                return;
            }
            Logger.Debug("OnBodyStart({0})", body);
            #if DEBUG
            if (body.healthComponent.godMode == false) {
                body.healthComponent.godMode = true;
            }
            #endif

            this.isSurvivorEnabled = ConVars.ConVars.disabledSurvivors.value.Contains(body.GetDisplayName()) == false;
            if (this.isSurvivorEnabled == false) {
                // isInitialised = true;
                Logger.Warn("Skills++ has been disable for this survivor. (survivorName = {0})", body.GetDisplayName());
                return;
            }

            var skillLocator = body.skillLocator;
            if (skillLocator == null) {
                Logger.Debug("Unable to initialise skill modifiers since there is no skill locator to use");
                return;
            }

            // isInitialised = true;
            foreach (GenericSkill genericSkill in skillLocator.FindAllGenericSkills()) {
                if (genericSkill == null) {
                    continue;
                }
                if (SkillModifierManager.HasSkillModifier(genericSkill.baseSkill) == false) {
                    Logger.Debug("SkillModifier for {0} does not exist", genericSkill.baseSkill);
                    continue;
                }

                SetSkillLevelForGenericSkill(genericSkill, 0);

                //SkillDef baseSkillDef = Instantiate(genericSkill.baseSkill);
                genericSkill.onSkillChanged -= this.OnSkillChanged;
                genericSkill.onSkillChanged += this.OnSkillChanged;

                OnSkillChanged(genericSkill);
                //genericSkill.SetBaseSkill(baseSkillDef);

                // #22
                // This mod is instantiating a clone of the generic skills current skill definition
                // This caused a bug for Acrid where its passive would not trigger due to the cloned skill def not being 
                // equal to other preset SkillDefs.
                // Here we access Acrid's damage controller and assign the skill definitions to uphold the equality 
                //if(body.TryGetComponent(out CrocoDamageTypeController crocoDamageTypeController)) {
                //    if(genericSkill.skillDef.skillName == crocoDamageTypeController.poisonSkillDef.skillName) {
                //        crocoDamageTypeController.poisonSkillDef = baseSkillDef;
                //    }
                //    if(genericSkill.skillDef.skillName == crocoDamageTypeController.blightSkillDef.skillName) {
                //        crocoDamageTypeController.blightSkillDef = baseSkillDef;
                //    }
                //}
            }

        }

        private int GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot) {
            if (this.isSurvivorEnabled == false) {
                return orig(self, slot);
            }

            int bonusSlots = 0;
            if (self == this.playerCharacterMasterController.master) {
                bonusSlots = EngiSkillModifier.GetDeployableSameSlotBonus(slot);
            }
            return orig(self, slot) + bonusSlots;
        }

        private void OnEnterState(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            if (isSurvivorEnabled) {
                if (GetSkillInfoAndModifierForState(self, out int skillLevel, out SkillDef skillDef, out BaseSkillModifier skillModifier)) {
                    skillModifier.OnSkillEnter(self, skillLevel);
                }
            }
            orig(self);
        }

        private void OnExitState(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if (isSurvivorEnabled && self is BaseState baseState) {
                if (GetSkillInfoAndModifierForState(baseState, out int skillLevel, out SkillDef skillDef, out BaseSkillModifier skillModifier)) {
                    skillModifier.OnSkillExit(baseState, skillLevel);
                }
            }
            orig(self);
        }

        private bool GetSkillInfoAndModifierForState(BaseState state, out int skillLevel, out SkillDef skillDef, out BaseSkillModifier skillModifier) {
            skillLevel = 0;
            skillDef = null;
            skillModifier = null;
            if (state == null) {
                return false;
            }
            var owningCharacterBody = FindOwningCharacterBody(state);
            if (owningCharacterBody != this.body) {
                return false;
            }
            var stateType = state.GetType();
            var tempSkillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(stateType);
            if (tempSkillModifier != null) {
                foreach (var skillName in tempSkillModifier.skillNames) {
                    var genericSkill = owningCharacterBody.skillLocator?.FindGenericSkillBySkillDef(skillName, true);
                    if (GetSkillInfoForGenericSkill(genericSkill, out skillLevel, out skillDef)) {
                        skillModifier = tempSkillModifier;
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        private CharacterBody FindOwningCharacterBody(BaseState state) {
            if (state.outer) {
                if (state.outer.TryGetComponent(out CharacterBody characterBody)) {
                    return characterBody;
                } else if (state.outer.TryGetComponent(out ProjectileController projectileController)) {
                    return projectileController.owner.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out MinionOwnership minionOwnership)) {
                    return minionOwnership.ownerMaster.GetBody();
                } else if (state.outer.TryGetComponent(out GenericOwnership genericOwnership)) {
                    return genericOwnership.ownerObject.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out Deployable deployable)) {
                    return deployable.ownerMaster.GetBody();
                }
            }
            return null;
        }

        public bool GetSkillInfoForGenericSkill(GenericSkill genericSkill, out int skillLevel, out SkillDef skillDef) {
            skillLevel = 0;
            skillDef = null;
            if (genericSkill == null) {
                return false;
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
                    return false;
                } else if (skillIndex != -1) {
                    skillDef = skillOverrides[skillIndex].skillDef;
                } else {
                    skillDef = genericSkill.baseSkill;
                }
            } catch {
                skillDef = genericSkill.baseSkill;
            }
            var baseSkillName = genericSkill?.baseSkill?.skillName;
            if (baseSkillName != null) {
                foreach (var skillUpgrade in this.skillUpgrades) {
                    if (skillUpgrade.skillName == baseSkillName) {
                        skillLevel = skillUpgrade.skillLevel;
                        return true;
                    }
                }
            }
            return false;
        }

        private void SetSkillLevelForGenericSkill(GenericSkill genericSkill, int newLevel) {
            var baseSkillName = genericSkill?.baseSkill?.skillName;
            if (baseSkillName != null) {
                for (int i = 0; i < this.skillUpgrades.Count; i++) {
                    if (this.skillUpgrades[i].skillName == baseSkillName) {
                        this.skillUpgrades[i] = new SkillUpgrade() {
                        skillName = baseSkillName,
                        skillLevel = newLevel
                        };
                        Logger.Debug("Updating {0} to level {1}", baseSkillName, newLevel);
                        return;
                    }
                }
                this.skillUpgrades.Add(new SkillUpgrade() {
                    skillName = baseSkillName,
                        skillLevel = newLevel
                });
                Logger.Debug("Setting {0} to level {1}", baseSkillName, newLevel);
            }
        }

        public bool CanUpgradeGenericSkill(GenericSkill genericSkill) {
            if (genericSkill == null) {
                return false;
            }
            if (unspentSkillPoints <= 0) {
                return false;
            }
            return GetSkillInfoForGenericSkill(genericSkill, out int skillLevel, out SkillDef skillDef) && SkillModifierManager.HasSkillModifier(skillDef);
        }

        private void OnSkillChanged(GenericSkill genericSkill) {
            if (GetSkillInfoForGenericSkill(genericSkill, out int skillLevel, out SkillDef skillDef)) {
                BaseSkillModifier modifier = SkillModifierManager.GetSkillModifier(skillDef);
                if (modifier != null) {
                    // get the skill level by the base skill name but provide the active skill def
                    modifier.OnSkillLeveledUp(skillLevel, this.body, skillDef);
                    genericSkill.RecalculateValues();
                }
            }
        }

        void Update() {
            #if DEBUG
            if (Input.GetKeyDown(KeyCode.Equals) && this.PlayerTeamIndex != TeamIndex.None) {
                //TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(500 * Time.deltaTime));
                //TeamManager.instance.SetTeamLevel(this.PlayerTeamIndex, TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex) + 1);
                Logger.Debug("Giving points");
                CmdGiveSkillPoint();
            }
            if (Input.GetKeyDown(KeyCode.Keypad1) && playerCharacterMasterController != null && body != null) {
                GameObject teleporter = GameObject.Find("Teleporter1(Clone)");
                Transform spawnLocation = body.transform;
                if (teleporter != null && teleporter.TryGetComponent(out TeleporterInteraction teleporterInteraction)) {
                    GameObject.Instantiate(teleporterInteraction.shopPortalSpawnCard.prefab, spawnLocation.position, spawnLocation.rotation, null);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.UtilitySkillMagazine);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.ExtraLife);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4) && this.playerCharacterMasterController != null) {
                this.body.skillLocator.ResetSkills();
            }
            if (Input.GetKeyDown(KeyCode.Keypad5) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("CIScepter");
            }
            if (Input.GetKeyDown(KeyCode.Keypad6) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.Syringe, 5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7) && this.playerCharacterMasterController != null) {
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.LunarPrimaryReplacement);
                this.playerCharacterMasterController.master?.inventory.GiveItem(ItemIndex.LunarUtilityReplacement);
            }
            #endif
        }

        public void OnBuySkill(GenericSkill genericSkill) {
            if (this.isServer) {
                this.PerformBuySkill(genericSkill);
            } else {
                var skillName = genericSkill?.baseSkill?.skillName;
                if (skillName != null) {
                    CmdBuySkill(skillName);
                }
            }
        }

        [Command]
        private void CmdBuySkill(string skillName) {
            GenericSkill genericSkill = this.skillLocator.FindGenericSkillBySkillDef(skillName, true);
            this.PerformBuySkill(genericSkill);
        }

        [Server]
        private void PerformBuySkill(GenericSkill genericSkill) {
            if (genericSkill == null) {
                return;
            }
            if (unspentSkillPoints <= 0) {
                Logger.Debug("Insufficient skill points to buy skill");
                return;
            }
            if (GetSkillInfoForGenericSkill(genericSkill, out int skillLevel, out SkillDef skillDef) && SkillModifierManager.HasSkillModifier(skillDef)) {
                unspentSkillPoints--;
                // increment and store the new skill level
                SetSkillLevelForGenericSkill(genericSkill, ++skillLevel);

                OnSkillChanged(genericSkill);
                return;
            }

        }

        [Server]
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

        [Command]
        private void CmdGiveSkillPoint() {
            this.earnedSkillPoints++;
            this.unspentSkillPoints++;
        }

        private int SkillPointsAtLevel(int characterLevel) {
            #if DEBUG
            return characterLevel - 1;
            #else
            if (levelsPerSkillPoint <= 1) {
                return characterLevel - 1;
            }
            return (characterLevel / this.levelsPerSkillPoint);
            #endif
        }
    }
}