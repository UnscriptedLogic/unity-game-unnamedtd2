using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterfaceManagement
{
    public class InspectWindow : MonoBehaviour
    {
        [Header("Inspect Window")]
        [SerializeField] private Image headerIcon;
        [SerializeField] private TextMeshProUGUI title;

        [Header("Path View")]
        [SerializeField] private Transform pathView;
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private GameObject segmentKnob;
        [SerializeField] private Color levelboughtColor;
        [SerializeField] private Color currentLevelColor;

        [Header("Upgrade Section")]
        [SerializeField] private GameObject upgradeSection;
        [SerializeField] private GameObject upgradeButtonPrefab;

        [Header("Sell Section")]
        [SerializeField] private TextMeshProUGUI sellCost;
        [SerializeField] private Button sellButton;

        [Header("Test")]
        [SerializeField] private TowerSO testtowerSO;
        [SerializeField] private int testlevel;

        private void Start()
        {
            ShowModal(testtowerSO, testlevel);
        }

        public void ShowModal(TowerSO towerSO, int level)
        {
            InitHeader(towerSO);
            InitUpgradeButtons(towerSO, level);
            InitSellButton(towerSO, level);
        }

        public void InitHeader(TowerSO towerSO)
        {
            headerIcon.sprite = towerSO.Icon;
            title.text = towerSO.TowerName;
        }

        public void InitUpgradeButtons(TowerSO towerSO, int levelIndex)
        {
            // Clear all the upgrade buttons
            foreach (Transform child in upgradeSection.transform)
            {
                Destroy(child.gameObject);
            }

            // Create the upgrade buttons
            TowerSO.UpgradeOption[] upgradeOptions;
            for (int i = 0; i < towerSO.TowerLevels[levelIndex].upgradeOptions.Length; i++)
            {
                upgradeOptions = towerSO.TowerLevels[levelIndex].upgradeOptions;
                GameObject upgradeButton = Instantiate(upgradeButtonPrefab, upgradeSection.transform);
                upgradeButton.GetComponent<UpgradeButton>().InitButton(upgradeOptions[i].nextUpgradeIcon, upgradeOptions[i].nextUpgradeDesc, upgradeOptions[i].nextUpgradeCost);

                //TODO: Implement Upgrade Logic
            }

            InitPathView(towerSO, levelIndex);
        }

        public void InitPathView(TowerSO towerSO, int levelIndex)
        {
            //Clear all segments
            for (int i = 0; i < pathView.childCount; i++)
            {
                Destroy(pathView.GetChild(0));
            }

            //Create Segments
            for (int i = 0; i < towerSO.TowerLevels.Length; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, pathView);
                if (i < levelIndex)
                {
                    segment.GetComponent<Image>().color = levelboughtColor;
                }
                else if (i == levelIndex)
                {
                    segment.GetComponent<Image>().color = currentLevelColor;
                }

                //Clear all knobs
                for (int j = 0; j < segment.transform.childCount; j++)
                {
                    Destroy(segment.transform.GetChild(0));
                }

                //Create Knobs
                for (int j = 0; j < towerSO.TowerLevels[i].upgradeOptions.Length; j++)
                {
                    GameObject knob = Instantiate(segmentKnob, segment.transform);
                }
            }
            
        }

        public void InitSellButton(TowerSO towerSO, int level)
        {
            sellCost.text = $"${towerSO.TowerLevels[level].sellCost}";
        }
    }
}