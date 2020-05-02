using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine.Events;

namespace Skills {

    delegate void UpgradeSkillEvent(SkillSlot skillSlot);

    [RequireComponent(typeof(SkillIcon))]
    class SkillLevelIconController : MonoBehaviour {

        private SkillIcon skillIcon;
        private GameObject CanBuyPanel;
        private GameObject UpgradeLevelText;
        private GameObject UpgradeLevelButton;

        private HGTextMeshProUGUI levelTextMesh;
        private HGButton buyButton;

        private CanvasRenderer CanBuyRenderer;

        public SkillSlot SkillSlot {
            get { return skillIcon.targetSkillSlot; }
        }

        public event UpgradeSkillEvent OnUpgradeSkill;

        private bool canUpgrade;
        private bool showBuyButton;

        void Awake() {
            this.skillIcon = GetComponent<SkillIcon>();
            this.CanBuyPanel = Instantiate(skillIcon.isReadyPanelObject, skillIcon.transform);
            this.CanBuyPanel.name = "CanBuyPanel";
            CanBuyPanel.transform.SetSiblingIndex(1);
            // tint the upgrade colour
            CanBuyRenderer = this.CanBuyPanel.GetComponent<CanvasRenderer>();

            // create the upgrade level label
            {
                UpgradeLevelText = new GameObject("LevelIndicator");
                UpgradeLevelText.transform.parent = skillIcon.transform;
                UpgradeLevelText.AddComponent<CanvasRenderer>();
                RectTransform textTransform = UpgradeLevelText.AddComponent<RectTransform>();

                this.levelTextMesh = UpgradeLevelText.AddComponent<HGTextMeshProUGUI>();
                levelTextMesh.text = "";
                levelTextMesh.fontSize = 24;
                levelTextMesh.color = Color.yellow;
                levelTextMesh.alignment = TMPro.TextAlignmentOptions.BottomLeft;
                levelTextMesh.enableWordWrapping = false;


                textTransform.ForceUpdateRectTransforms();
                textTransform.localScale = Vector3.one; // fixes multiplayer bug where joining players have overscaled buttons
                textTransform.localPosition = Vector2.zero;
                textTransform.anchorMin = Vector2.zero;
                textTransform.anchorMax = Vector2.zero;
                textTransform.anchoredPosition = new Vector2(0, 0); // bottom right corner
                textTransform.sizeDelta = Vector2.zero;
                textTransform.offsetMin = new Vector2(0, -4);
                textTransform.offsetMax = new Vector2(0, -4);
                textTransform.ForceUpdateRectTransforms();
            }

            // create the clickable upgrade button
            {

                UpgradeLevelButton = new GameObject("BuySkillButton");
                UpgradeLevelButton.transform.parent = skillIcon.transform;
                UpgradeLevelButton.AddComponent<CanvasRenderer>();
                RectTransform buttonTransform = UpgradeLevelButton.AddComponent<RectTransform>();

                MPEventSystemLocator eventSystemLocation = UpgradeLevelButton.AddComponent<MPEventSystemLocator>();

                HGTextMeshProUGUI buttonTextMesh = UpgradeLevelButton.AddComponent<HGTextMeshProUGUI>();
                buttonTextMesh.text = "Buy";
                buttonTextMesh.fontSize = 24;
                buttonTextMesh.color = Color.yellow;
                buttonTextMesh.alignment = TMPro.TextAlignmentOptions.Center;
                buttonTextMesh.enableWordWrapping = false;

                buyButton = UpgradeLevelButton.AddComponent<HGButton>();
                buyButton.onClick.AddListener(() => {
                    this.OnUpgradeSkill.Invoke(this.SkillSlot);
                });

                buttonTransform.ForceUpdateRectTransforms();
                buttonTransform.localScale = Vector3.one; // fixes multiplayer bug where joining players have overscaled buttons
                buttonTransform.localPosition = Vector2.zero;
                buttonTransform.anchorMin = Vector2.zero;
                buttonTransform.anchorMax = Vector2.one;
                buttonTransform.anchoredPosition = new Vector2(0, 0); // bottom right corner
                buttonTransform.sizeDelta = Vector2.zero;
                buttonTransform.offsetMin = new Vector2(0, 0);
                buttonTransform.offsetMax = new Vector2(0, 0);
                buttonTransform.ForceUpdateRectTransforms(); 

                // ButtonSkinController skinController = UpgradeLevelButton.AddComponent<ButtonSkinController>();
                // skinController.skinData =
            }

            //CanBuyRenderer.SetColor(Color.yellow);
            SetCanUpgrade(false);
        }

        public void SetCanUpgrade(bool canUpgrade) {
            this.canUpgrade = canUpgrade;
            CanBuyPanel.SetActive(canUpgrade);
            CanBuyRenderer.SetColor(Color.yellow);
            if ((showBuyButton && canUpgrade) != UpgradeLevelButton.activeInHierarchy) {
                UpgradeLevelButton.SetActive(showBuyButton && canUpgrade);
            }
        }

        public void ShowBuyButton(bool showButton) {
            this.showBuyButton = showButton;
            if ((showBuyButton && canUpgrade) != UpgradeLevelButton.activeInHierarchy) {
                UpgradeLevelButton.SetActive(showBuyButton && canUpgrade);
            }
        }

        public void SetLevel(int level) {
            string newText = "";
            for(int i = 1; i < level; i++) {
                newText += "+";
            }
            levelTextMesh.text = newText;
        }

    }
}
