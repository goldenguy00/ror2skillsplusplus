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
        private GameObject UpgradeLevelText;
        private GameObject UpgradeLevelButton;

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

            // create the upgrade level label
            UpgradeLevelText = new GameObject();
            UpgradeLevelText.transform.parent = skillIcon.transform;
            UpgradeLevelText.AddComponent<CanvasRenderer>();
            RectTransform textTransform = UpgradeLevelText.AddComponent<RectTransform>();

            HGTextMeshProUGUI textMesh = UpgradeLevelText.AddComponent<HGTextMeshProUGUI>();
            textMesh.text = "";
            for(int i = UnityEngine.Random.Range(0, 4); i > 0; i--) {
                textMesh.text += "+";
            }
            textMesh.fontSize = 24;
            textMesh.color = Color.yellow;
            textMesh.alignment = TMPro.TextAlignmentOptions.BottomLeft;
            textMesh.enableWordWrapping = false;


            textTransform.ForceUpdateRectTransforms();
            textTransform.localPosition = Vector2.zero;
            textTransform.anchorMin = Vector2.zero;
            textTransform.anchorMax = Vector2.zero;
            textTransform.anchoredPosition = new Vector2(0, 0); // bottom right corner
            textTransform.sizeDelta = Vector2.zero;
            textTransform.offsetMin = new Vector2(0, -4);
            textTransform.offsetMax = new Vector2(0, -4);
            textTransform.ForceUpdateRectTransforms();


            // create the clickable upgrade button



            SetCanUpgrade(false);
        }

        public void SetCanUpgrade(bool canUpgrade) {
            this.canUpgrade = canUpgrade;
            CanBuyPanel.SetActive(canUpgrade);
            CanBuyRenderer.SetColor(Color.yellow);
        }

    }
}
