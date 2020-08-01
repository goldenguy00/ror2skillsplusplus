using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using R2API.Utils;

using RoR2;
using RoR2.ConVar;

using UnityEngine;

namespace SkillsPlusPlus.ConVars {
    internal static class ConVars {

        internal static StringListConVar disabledSurvivors = new StringListConVar("spp_disabled_survivors", ConVarFlags.Archive, new List<string>(), "The list of survivors excluded from Skills++ behaviour in a string form");

        [ConCommand(commandName = "spp_disable_survivor", flags = ConVarFlags.None, helpText = "Prints all of the survivors that are exlcuded from any Skills++ behaviour")]
        public static void CCDisableSurvivor(ConCommandArgs args) {
            string survivorName = args.TryGetArgString(0);
            if(survivorName == null) {
                Debug.Log("Could not parse a survivor name");
                return;
            }
            bool didFindSurvivor = false;
            foreach(SurvivorDef survivorDef in SurvivorCatalog.allSurvivorDefs) {
                if(survivorName.Equals(survivorDef.displayNameToken, StringComparison.OrdinalIgnoreCase)) {
                    if(disabledSurvivors.value.Contains(survivorDef.displayNameToken)) {
                        Debug.LogFormat("{0} has already been disabled", survivorDef.displayNameToken);
                        return;
                    } else {
                        didFindSurvivor = true;
                        disabledSurvivors.value.Add(survivorDef.displayNameToken);
                    }
                }
            }
            if(didFindSurvivor) {
                Debug.Log(disabledSurvivors.GetString());
            } else {
                Debug.LogFormat("Could not find any survivors named {0}", survivorName);
            }
        }

        [ConCommand(commandName = "spp_enable_survivor", flags = ConVarFlags.None, helpText = "Prints all of the survivors that are exlcuded from any Skills++ behaviour")]
        public static void CCEnableSurvivor(ConCommandArgs args) {
            string survivorName = args.TryGetArgString(0);
            if(survivorName == null) {
                Debug.Log("Could not parse a survivor name");
                return;
            }
            foreach(SurvivorDef survivorDef in SurvivorCatalog.allSurvivorDefs) {
                if(survivorName.Equals(survivorDef.displayNameToken, StringComparison.OrdinalIgnoreCase)) {
                    disabledSurvivors.value.Remove(survivorDef.displayNameToken);
                }
            }
            Debug.Log(disabledSurvivors.GetString());
        }
    }

    internal class StringListConVar: BaseConVar {

        public List<string> value { get; protected set; }

        public StringListConVar(string name, ConVarFlags flags, List<string> defaultValue, string helpText): base(name, flags, "", helpText) {
            this.value = defaultValue;
        }

        public override string GetString() {
            return value.Join();
        }

        public override void SetString(string newValue) {
            this.value = newValue.Split(',').Select((item) => {
                return item.Trim();
            }).ToList();
        }
    }
}
