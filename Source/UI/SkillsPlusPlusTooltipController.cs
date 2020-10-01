using System;
using System.Reflection.Emit;
using R2API.MiscHelpers;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace SkillsPlusPlus.UI {

    [RequireComponent(typeof(TooltipController))]
    class SkillsPlusPlusTooltipController : MonoBehaviour {

        TooltipController tooltipController;

        public string skillName;

        void Awake() {
            this.tooltipController = GetComponent<TooltipController>();

        }

        void Start() {
            var panelTransform = this.tooltipController.tooltipFlipTransform;
            GameObject bodyRectGameObject = this.tooltipController.bodyLabel.transform.parent.gameObject;
            GameObject skillUpgradeRect = Instantiate(bodyRectGameObject, panelTransform);
            skillUpgradeRect.transform.SetSiblingIndex(Math.Max(0, panelTransform.childCount - 2));

            var modifier = SkillModifierManager.GetSkillModifierByName(skillName);
            string token = modifier?.skillUpgradeDescriptionToken;
            skillUpgradeRect.SetActive(token != null);

            if (token != null) {
                #if DEBUG 
                var description = "[" + modifier.GetType().Name + "]" + Language.GetString(token);
                #else
                var description = Language.GetString(token);
                #endif
                var label = skillUpgradeRect.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                label.text = description;
            }
        }

    }

}