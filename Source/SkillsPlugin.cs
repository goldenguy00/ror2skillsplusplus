using BepInEx;
using R2API;
using R2API.Utils;
using RoR2.UI;
using SkillsPlusPlus.Modifiers;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus {

    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.cwmlolzlz.skills", "Skills", "0.1.6")]
    [R2APISubmoduleDependency(nameof(CommandHelper), nameof(LanguageAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod)]
    public sealed class SkillsPlugin : BaseUnityPlugin {

        private SkillPointsController localSkillPointsController;
        private GameObject modLabel;

        //private SkillPointsController skillsController;
        //private SkillLevelIconController[] skillsHUDControllers;

        void Awake() {

            SkillsPlusPlus.Logger.Warn(@"
  _____  _     _  _  _                       ____         _         
 / ____|| |   (_)| || |        _      _     |  _ \       | |        
| (___  | | __ _ | || | ___  _| |_  _| |_   | |_) |  ___ | |_  __ _ 
 \___ \ | |/ /| || || |/ __||_   _||_   _|  |  _ <  / _ \| __|/ _` |
 ____) ||   < | || || |\__ \  |_|    |_|    | |_) ||  __/| |_| (_| |
|_____/ |_|\_\|_||_||_||___/                |____/  \___| \__|\__,_|

Note: You are running the Skills++ {0} beta.
This is a pre-release and is to guarenteed to be stable, bug free, or crash free.

Raise bugs here https://discord.gg/wU94CjJ
            ", this.Info.Metadata.Version.ToString());

            #if DEBUG
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.Debug;
            UnityEngine.Networking.LogFilter.currentLogLevel = LogFilter.Info;

            // disable client authing when connecting to a server to allow two game instances to run in parallel
            // On.RoR2.Networking.GameNetworkManager.ClientSendAuth += (orig, self, connection) => { };

            On.RoR2.Console.InitConVars += (orig, self) => {
                orig(self);
                RoR2.Console.instance.SubmitCmd(null, "splash_skip 1", false);
                RoR2.Console.instance.SubmitCmd(null, "intro_skip 1", false);
            };

            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) => {
                return true;
            };

            bool didAttemptToConnect = false;
            On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
                if (didAttemptToConnect == false) {
                    didAttemptToConnect = true;
                    RoR2.Console.instance.SubmitCmd(null, "connect 192.168.1.102:27015", true);
                }
                orig(self);
            };
            #endif

            CommandHelper.AddToConsoleWhenReady();

            LoaderKnucklesSkillModifier.PatchSkillName();
            LoaderThrowPylonSkillModifier.PatchSkillName();

            SkillModifierManager.LoadSkillModifiers();
            SkillInput.SetupCustomInput();
            SkillOptions.SetupGameplayOptions();

            GameObject playerMasterPrefab = Resources.Load<GameObject>("prefabs/charactermasters/CommandoMaster");
            playerMasterPrefab.EnsureComponent<SkillPointsController>();

            On.RoR2.UI.HUD.Awake += this.HUD_Awake;
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self) {
            orig(self);
            self.GetComponentsInChildren<SkillIcon>(true).ForEachTry(skillIcon => {
                skillIcon.EnsureComponent<SkillLevelIconController>();
            });
        }
    }
}