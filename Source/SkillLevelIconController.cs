using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using RoR2;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using UnityEngine.Events;
using R2API.Utils;
using UnityEngine.UI;

namespace SkillsPlusPlus {

    delegate void UpgradeSkillEvent(string skillName);

    [RequireComponent(typeof(SkillIcon))]
    sealed class SkillLevelIconController : MonoBehaviour {

        private SkillIcon skillIcon;
        private GameObject CanBuyPanel;
        private GameObject LevelBackgroundPanel;
        private GameObject LevelText;
        private GameObject UpgradeButton;

        private HGTextMeshProUGUI levelTextMesh;
        private HGButton buyButton;

        private CanvasRenderer CanBuyRenderer;

        public string skillName {
            get { return skillIcon.targetSkill?.skillDef?.skillName; }
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
                LevelBackgroundPanel = new GameObject("Skills++ Level Background Panel");
                LevelBackgroundPanel.transform.parent = skillIcon.transform;
                LevelBackgroundPanel.transform.localPosition = Vector3.zero;
                LevelBackgroundPanel.transform.localScale = Vector3.one;

                RectTransform backgroundTransform = LevelBackgroundPanel.AddComponent<RectTransform>();
                backgroundTransform.offsetMin = Vector2.zero;
                backgroundTransform.offsetMax = Vector2.zero;
                backgroundTransform.anchorMin = new Vector2(0.5f, 0.25f);
                backgroundTransform.anchorMax = new Vector2(0.5f, 0.25f);
                backgroundTransform.ForceUpdateRectTransforms();

                RawImage backgroundImage = LevelBackgroundPanel.AddComponent<RawImage>();
                Texture2D texture = new Texture2D(1, 1);
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                backgroundImage.texture = texture;
                backgroundImage.color = new Color(0, 0, 0, 0.7f);
                backgroundImage.SetNativeSize();

                ContentSizeFitter contentFitter = LevelBackgroundPanel.AddComponent<ContentSizeFitter>();
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                HorizontalLayoutGroup layoutGroup = LevelBackgroundPanel.AddComponent<HorizontalLayoutGroup>();
                layoutGroup.padding = new RectOffset(4, 4, 0, 0);
                layoutGroup.childAlignment = TextAnchor.MiddleCenter;

                LevelText = new GameObject("LevelIndicator");
                LevelText.transform.parent = LevelBackgroundPanel.transform;
                LevelText.AddComponent<CanvasRenderer>();
                RectTransform textTransform = LevelText.AddComponent<RectTransform>();

                this.levelTextMesh = LevelText.AddComponent<HGTextMeshProUGUI>();
                levelTextMesh.text = "";
                levelTextMesh.fontSize = 16;
                levelTextMesh.color = Color.yellow;
                levelTextMesh.alignment = TMPro.TextAlignmentOptions.Center;
                levelTextMesh.enableWordWrapping = false;


                textTransform.ForceUpdateRectTransforms();
                textTransform.localScale = Vector3.one; // fixes multiplayer bug where joining players have overscaled buttons
                textTransform.localPosition = Vector2.zero;
                textTransform.anchorMin = Vector2.zero;
                textTransform.anchorMax = Vector2.one;
                // textTransform.anchoredPosition = new Vector2(0.5f, 0); // bottom center corner
                textTransform.sizeDelta = Vector2.zero;
                textTransform.offsetMin = Vector2.zero;
                textTransform.offsetMax = Vector2.zero;
                textTransform.ForceUpdateRectTransforms();
            }

            // create the clickable upgrade button
            {

                UpgradeButton = new GameObject("BuySkillButton");
                UpgradeButton.transform.parent = skillIcon.transform;
                UpgradeButton.AddComponent<CanvasRenderer>();
                RectTransform buttonTransform = UpgradeButton.AddComponent<RectTransform>();

                MPEventSystemLocator eventSystemLocation = UpgradeButton.AddComponent<MPEventSystemLocator>();

                HGTextMeshProUGUI buttonTextMesh = UpgradeButton.AddComponent<HGTextMeshProUGUI>();
                buttonTextMesh.text = "Buy";
                buttonTextMesh.fontSize = 24;
                buttonTextMesh.color = Color.yellow;
                buttonTextMesh.alignment = TMPro.TextAlignmentOptions.Center;
                buttonTextMesh.enableWordWrapping = false;

                buyButton = UpgradeButton.AddComponent<HGButton>();
                buyButton.onClick.AddListener(() => {
                    this.OnUpgradeSkill.Invoke(this.skillName);
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
            if ((showBuyButton && canUpgrade) != UpgradeButton.activeInHierarchy) {
                UpgradeButton.SetActive(showBuyButton && canUpgrade);
            }
        }

        public void ShowBuyButton(bool showButton) {
            this.showBuyButton = showButton;
            if ((showBuyButton && canUpgrade) != UpgradeButton.activeInHierarchy) {
                UpgradeButton.SetActive(showBuyButton && canUpgrade);
            }
        }

        public void SetLevel(int level) {
            // only show the level if it is non-zero
            levelTextMesh.text = level != 0 ? level.ToString() : null;
        }

    }
}
