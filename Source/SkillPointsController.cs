using System;
using System.Collections.Generic;
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

        private CharacterBody body {
            get { return playerCharacterMasterController.master?.GetBody(); }
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

        [SyncVar]
        private int earnedSkillPoints = 0;

        [SyncVar]
        private int unspentSkillPoints = 0;

        [SyncVar]
        private int levelsPerSkillPoint = 5;

        public bool hasUnspentPoints {
            get { return unspentSkillPoints > 0; }
        }

        private bool isSurvivorEnabled = true;

        private Dictionary<string, int> transferrableSkillUpgrades = new Dictionary<string, int>();

        void Awake() {

            this.levelsPerSkillPoint = ConVars.ConVars.levelsPerSkillPoint.value;

            this.playerCharacterMasterController = this.GetComponent<PlayerCharacterMasterController>();
            this.playerCharacterMasterController.master.onBodyStart += OnBodyStart;
            // On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += GetDeployableSameSlotLimit;
            // On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateStats;
            // On.EntityStates.GenericCharacterMain.CanExecuteSkill += this.GenericCharacterMain_CanExecuteSkill;
        }

        [Server]
        internal void PersistUpgrade(int skillLevel, string targetBaseSkillName) {
            if (targetBaseSkillName == null) {
                return;
            }
            transferrableSkillUpgrades[targetBaseSkillName] = skillLevel;
        }

        [Server]
        void OnBodyStart(CharacterBody body) {
            // attempt to transfer and apply skill levels
            var skillUpgrades = body.GetComponents<SkillUpgrade>();
            foreach (var skillUpgrade in skillUpgrades) {
                if (skillUpgrade.targetBaseSkillName != null && transferrableSkillUpgrades.ContainsKey(skillUpgrade.targetBaseSkillName)) {
                    skillUpgrade.skillLevel = transferrableSkillUpgrades[skillUpgrade.targetBaseSkillName];
                    transferrableSkillUpgrades.Remove(skillUpgrade.targetBaseSkillName);
                }
            }

            foreach (var remainingSkillUpgrades in transferrableSkillUpgrades) {
                this.unspentSkillPoints += remainingSkillUpgrades.Value;
            }

            transferrableSkillUpgrades.Clear();
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

        #endregion        

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

        public void DeductSkillPoints(int amount) {
            this.unspentSkillPoints -= amount;
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