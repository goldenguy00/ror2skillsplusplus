using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EntityStates;
using R2API.Utils;
using Rewired;
using RiskOfOptions;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus
{

    [RequireComponent(typeof(PlayerCharacterMasterController))]
    [RequireComponent(typeof(NetworkIdentity))]
    public sealed class SkillPointsController : NetworkBehaviour
    {

        // private const int SKILL_DISABLED = -1;

        private PlayerCharacterMasterController playerCharacterMasterController;

        private CharacterBody body
        {
            get { return playerCharacterMasterController.master?.GetBody(); }
        }
        private SkillLocator skillLocator
        {
            get { return body?.skillLocator; }
        }

        private TeamIndex PlayerTeamIndex
        {
            get
            {
                if (playerCharacterMasterController.master.hasBody)
                {
                    return playerCharacterMasterController.master.GetBody().teamComponent.teamIndex;
                }
                return TeamIndex.None;
            }
        }

        [SyncVar]
        public bool isSurvivorEnabled;

        [SyncVar]
        private int earnedSkillPoints = 0;

        [SyncVar]
        private int unspentSkillPoints = 0;

        [SyncVar]
        private int levelsPerSkillPoint = ConVars.ConVars.levelsPerSkillPoint.value;

        [SyncVar]
        public bool multScalingLinear = false;

        public bool hasUnspentPoints
        {
            get { return unspentSkillPoints > 0; }
        }

        private Dictionary<string, int> transferrableSkillUpgrades = new Dictionary<string, int>();

        void Awake()
        {
            multScalingLinear = ConVars.ConVars.multScalingLinear.value;
            Logger.Debug("levelsPerSkillPoint: {0}", this.levelsPerSkillPoint);

            this.playerCharacterMasterController = this.GetComponent<PlayerCharacterMasterController>();
        }

        void OnEnable()
        {
            this.playerCharacterMasterController.master.onBodyStart += this.OnBodyStart;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += this.GetDeployableSameSlotLimit;
            On.EntityStates.GenericCharacterMain.CanExecuteSkill += this.GenericCharacterMain_CanExecuteSkill;
            if (NetworkServer.active)
            {
                On.RoR2.CharacterBody.RecalculateStats += this.OnRecalculateStats;
            }
        }

        void OnDisable()
        {
            this.playerCharacterMasterController.master.onBodyStart -= this.OnBodyStart;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit -= this.GetDeployableSameSlotLimit;
            On.EntityStates.GenericCharacterMain.CanExecuteSkill -= this.GenericCharacterMain_CanExecuteSkill;
            On.RoR2.CharacterBody.RecalculateStats -= this.OnRecalculateStats;
        }

        [Server]
        internal void PersistUpgrade(int skillLevel, string targetBaseSkillName)
        {
            if (targetBaseSkillName == null)
            {
                return;
            }
            transferrableSkillUpgrades[targetBaseSkillName] = skillLevel;
        }

        void OnBodyStart(CharacterBody body)
        {
            this.isSurvivorEnabled = !ConVars.ConVars.disabledSurvivors.value.Contains(body.GetDisplayName());
            Logger.Debug("OnBodyStart({0})", body);
            // attempt to transfer and apply skill levels
            var skillUpgrades = body.GetComponents<SkillUpgrade>();
            foreach (var skillUpgrade in skillUpgrades)
            {
                skillUpgrade.SetSkillPointsController(this);
            }
            TransferSkillUpgrades(body);

#if DEBUG
            body.healthComponent.godMode = true;
#endif
        }


        bool bMorphedToHeretic = false;

        [Server]
        private void TransferSkillUpgrades(CharacterBody body)
        {
            unspentSkillPoints = SkillPointsAtLevel((int)body.level);

            //If this is the first time we morph into heretic, Heretic skill levels will be reset to allow respec'ing.
            bool bFirstHereticMorph = body.baseNameToken == "HERETIC_BODY_NAME" && !bMorphedToHeretic;

            var skillUpgrades = body.GetComponents<SkillUpgrade>();
            foreach (var skillUpgrade in skillUpgrades)
            {
                if (!isSurvivorEnabled) break;
                if (skillUpgrade.targetBaseSkillName != null && transferrableSkillUpgrades.ContainsKey(skillUpgrade.targetBaseSkillName))
                {
                    skillUpgrade.skillLevel = (bFirstHereticMorph ? 0 : transferrableSkillUpgrades[skillUpgrade.targetBaseSkillName]);
                    unspentSkillPoints -= skillUpgrade.skillLevel;
                    transferrableSkillUpgrades.Remove(skillUpgrade.targetBaseSkillName);
                    //skillUpgrade.OnBuySkill();
                }
            }

            if (bFirstHereticMorph)
            {
                bMorphedToHeretic = true;
            }
        }

        #region Hooks

        private bool GenericCharacterMain_CanExecuteSkill(On.EntityStates.GenericCharacterMain.orig_CanExecuteSkill orig, GenericCharacterMain self, GenericSkill skillSlot)
        {
            if (this.isSurvivorEnabled && this.body != null && self.outer.commonComponents.characterBody == this.body)
            {
                Player inputPlayer = this.playerCharacterMasterController?.networkUser?.localUser?.inputPlayer;
                if (inputPlayer != null && ConVars.ConVars.buySkillsKeybind.IsPressedInclusive() && ConVars.ConVars.disableOnBuy.value && unspentSkillPoints > 0)
                {
                    return false;
                }
            }
            return orig(self, skillSlot);
        }

        private int GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            int bonusSlots = 0;
            if (this.isSurvivorEnabled && self == this.playerCharacterMasterController.master)
            {
                bonusSlots = EngiSkillModifier.GetDeployableSameSlotBonus(slot);
            }
            return orig(self, slot) + bonusSlots;
        }

        private void OnRecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self == this.body)
            {
                this.OnLevelChanged();
            }
        }

        #endregion        

        void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.Equals) && this.PlayerTeamIndex != TeamIndex.None)
            {
                //TeamManager.instance.GiveTeamExperience(body.teamComponent.teamIndex, (ulong)(500 * Time.deltaTime));
                //TeamManager.instance.SetTeamLevel(this.PlayerTeamIndex, TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex) + 1);
                CmdGiveSkillPoint();
            }
            if (Input.GetKeyDown(KeyCode.Keypad1) && playerCharacterMasterController != null && body != null)
            {
                GameObject teleporter = GameObject.Find("Teleporter1(Clone)");
                Transform spawnLocation = body.transform;
                if (teleporter != null && teleporter.TryGetComponent(out TeleporterInteraction teleporterInteraction))
                {
                    var portalGameObject = GameObject.Instantiate(teleporterInteraction.shopPortalSpawnCard.prefab, spawnLocation.position, spawnLocation.rotation, null);
                    NetworkServer.Spawn(portalGameObject);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) && this.playerCharacterMasterController != null)
            {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("UtilitySkillMagazine");
            }
            if (Input.GetKeyDown(KeyCode.Keypad3) && this.playerCharacterMasterController != null)
            {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("ExtraLife");
            }
            if (Input.GetKeyDown(KeyCode.Keypad4) && this.playerCharacterMasterController != null)
            {
                this.body.skillLocator.ResetSkills();
            }
            if (Input.GetKeyDown(KeyCode.Keypad5) && this.playerCharacterMasterController != null)
            {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("CIScepter");
            }
            if (Input.GetKeyDown(KeyCode.Keypad6) && this.playerCharacterMasterController != null)
            {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("Syringe", 5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7) && this.playerCharacterMasterController != null)
            {
                this.playerCharacterMasterController.master?.inventory.GiveItemString("LunarPrimaryReplacement");
                this.playerCharacterMasterController.master?.inventory.GiveItemString("LunarUtilityReplacement");
                this.playerCharacterMasterController.master?.inventory.GiveItemString("LunarSecondaryReplacement");
                this.playerCharacterMasterController.master?.inventory.GiveItemString("LunarSpecialReplacement");
            }
#endif
        }

        public void DeductSkillPoints(int amount)
        {
            this.unspentSkillPoints -= amount;
        }

        [Server]
        public void OnLevelChanged()
        {
            int characterLevel = (int)TeamManager.instance.GetTeamLevel(this.PlayerTeamIndex);
            // Logger.Debug("OnLevelChanged({0}) for team {1}", characterLevel, PlayerTeamIndex);
            if (this.PlayerTeamIndex == TeamIndex.None)
            {
                return;
            }

            levelsPerSkillPoint = ConVars.ConVars.levelsPerSkillPoint.value;
            int newSkillPoints = Math.Max(0, SkillPointsAtLevel(characterLevel) - earnedSkillPoints);

            earnedSkillPoints += newSkillPoints;
            unspentSkillPoints += newSkillPoints;
        }

        [Command]
        [Server]
        private void CmdGiveSkillPoint()
        {
            Logger.Debug("Giving points");
            this.earnedSkillPoints++;
            this.unspentSkillPoints++;
        }

        private int SkillPointsAtLevel(int characterLevel)
        {
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