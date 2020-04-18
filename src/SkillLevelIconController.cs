using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using RoR2;
using RoR2.UI;

namespace Skills {

    [RequireComponent(typeof(SkillIcon))]
    class SkillLevelIconController : MonoBehaviour {

        private SkillIcon skillIcon;
        private GameObject CanBuyPanel;

        private CanvasRenderer CanBuyRenderer;

        public SkillSlot SkillSlot {
            get { return skillIcon.targetSkillSlot; }
        }
        
        private bool canUpgrade;


        void Awake() {
            this.skillIcon = GetComponent<SkillIcon>();
            this.CanBuyPanel = Instantiate(skillIcon.isReadyPanelObject, skillIcon.transform);
            this.CanBuyPanel.name = "CanBuyPanel";
            CanBuyPanel.transform.SetSiblingIndex(1);
            // tint the upgrade colour
            CanBuyRenderer = this.CanBuyPanel.GetComponent<CanvasRenderer>();
            // CanBuyRenderer.SetColor(ColorCatalog.GetColor(ColorCatalog.ColorIndex.Interactable));
            CanBuyRenderer.SetColor(Color.yellow);
            SetCanUpgrade(false);
        }

        public void SetCanUpgrade(bool canUpgrade) {
            this.canUpgrade = canUpgrade;
            CanBuyPanel.SetActive(canUpgrade);
            CanBuyRenderer.SetColor(Color.yellow);
        }


    }
}
