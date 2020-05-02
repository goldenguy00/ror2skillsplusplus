using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using R2API;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using R2API.Utils;
using MonoMod.Cil;

namespace Skills {

    [RequireComponent(typeof(CharacterSelectController))]
    class SkillModifierTooltipController : MonoBehaviour {

        private CharacterSelectController characterSelectController;

        public void Awake() {

            this.characterSelectController = GetComponent<CharacterSelectController>();

            On.RoR2.UI.CharacterSelectController.RebuildLocal += this.OnRebuildLocal;
        }

        void OnDestroy() {
            On.RoR2.UI.CharacterSelectController.RebuildLocal -= this.OnRebuildLocal;
        }

        public void OnRebuildLocal(On.RoR2.UI.CharacterSelectController.orig_RebuildLocal orig, CharacterSelectController characterSelectController) {
            orig(characterSelectController);

            if (characterSelectController == this.characterSelectController) {

                // get the selected character game object and components
                int bodyIndexFromSurvivorIndex = SurvivorCatalog.GetBodyIndexFromSurvivorIndex(characterSelectController.selectedSurvivorIndex);
                GameObject bodyPrefab = BodyCatalog.GetBodyPrefab(bodyIndexFromSurvivorIndex);
                SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
                GenericSkill[] genericSkills = bodyPrefab.GetComponents<GenericSkill>();

                UIElementAllocator<RectTransform> skillStripAllocator = characterSelectController.GetFieldValue<UIElementAllocator<RectTransform>>("skillStripAllocator");
                for (int i = 0; i < skillStripAllocator.elements.Count; i++) {
                    
                    RectTransform skillStripTransform = skillStripAllocator.elements[i];
                    GameObject skillStripGameObject = skillStripTransform.gameObject;

                    //HGTextMeshProUGUI component2 = skillStripTransform.Find("Inner/SkillDescriptionPanel/SkillName").GetComponent<HGTextMeshProUGUI>();
                    HGTextMeshProUGUI skillDescriptionText = skillStripTransform.Find("Inner/SkillDescriptionPanel/SkillDescription").GetComponent<HGTextMeshProUGUI>();

                    int skillIndex = i;

                    // assumes passive skills are always displayed in the first row of the loadout
                    if (skillLocator.passiveSkill.enabled) {
                        if (i == 0) {
                            continue;
                        }
                        skillIndex--;
                    }

                    // assumes the order of the generic skills matches the order displayed in the UI
                    if (skillIndex < genericSkills.Length) {
                        GenericSkill genericSkill = genericSkills[skillIndex];

                        LocalUser localUser = characterSelectController.GetFieldValue<LocalUser>("localUser");
                        if (localUser != null && localUser.userProfile != null) {
                            Loadout loadout = new Loadout();
                            localUser.userProfile.CopyLoadout(loadout);

                            uint variantIndex = loadout.bodyLoadoutManager.GetSkillVariant(bodyIndexFromSurvivorIndex, skillIndex);
                            SkillDef skillDef = genericSkill.skillFamily.variants[variantIndex].skillDef;

                            ISkillModifier skillModifier = SkillModifierManager.GetSkillModifier(skillDef);
                            string overrideDescriptionToken = skillModifier.GetOverrideSkillDescriptionToken();
                            if (overrideDescriptionToken != null) {
                                string overrideDescription = Language.GetString(overrideDescriptionToken);
                                if (overrideDescription != null) {
                                    skillDescriptionText.text = overrideDescription;
                                } else {
                                    skillDescriptionText.text = overrideDescriptionToken;
                                }
                            }
                        }
                    } else {
                        Logger.Warn("BUG: Attempting to reference out of bounds skill at index %d", skillIndex);
                    }
                    
                }                
            }
        }
    }
}
