using GameManagement;
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
        [SerializeField] private ContentSizeFitter contentSizeFitter;

        [Header("Inspect Window")]
        [SerializeField] private Image headerIcon;
        [SerializeField] private TextMeshProUGUI title;

        [Header("Path View")]
        [SerializeField] private Transform pathView;
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private GameObject segmentKnob;
        [SerializeField] private Color currentLevelColor;
        [SerializeField] private Color upgradeBoughtColor;

        [Header("Upgrade Section")]
        [SerializeField] private GameObject upgradeSection;
        [SerializeField] private GameObject upgradeButtonPrefab;
        [SerializeField] private GameObject fullyUpgraded;

        [Header("Sell Section")]
        [SerializeField] private TextMeshProUGUI sellCost;
        [SerializeField] private Button sellButton;

        public void ShowModal(TowerSO towerSO, List<int> upgradeHistory)
        {
            Clear();
            InitHeader(towerSO);
        }

        public void Clear()
        {
            fullyUpgraded.SetActive(false);

            // Clear all the upgrade buttons
            for (int i = upgradeSection.transform.childCount - 1; i > -1; i--)
            {
                LevelManagement.PushObject(upgradeSection.transform.GetChild(i).gameObject);
                //Destroy(upgradeSection.transform.GetChild(i).gameObject);
            }

            //Clear all segments
            for (int i = 0; i < pathView.childCount; i++)
            {
                Destroy(pathView.GetChild(i).gameObject);
            }
        }

        public void InitHeader(TowerSO towerSO)
        {
            headerIcon.sprite = towerSO.Icon;
            title.text = towerSO.TowerName;
        }

        public Button[] InitUpgradeButtons(TowerSO towerSO, int[] upgradeHistory)
        {
            // Create the upgrade buttons
            int levelIndex = upgradeHistory.Length;
            TowerSO.UpgradeOption[] upgradeOptions;

            if (upgradeHistory.Length == towerSO.TowerLevels.Length)
            {
                //All upgrades completed
                fullyUpgraded.SetActive(true);
                InitPathView(towerSO, levelIndex, upgradeHistory);
                return null;
            }

            Button[] upgradeButtons = new Button[towerSO.TowerLevels[levelIndex].upgradeOptions.Length];
            for (int i = 0; i < towerSO.TowerLevels[levelIndex].upgradeOptions.Length; i++)
            {
                upgradeOptions = towerSO.TowerLevels[levelIndex].upgradeOptions;
                GameObject upgradeButton = LevelManagement.PullObject(upgradeButtonPrefab, Vector3.zero, Quaternion.identity, true, upgradeSection.transform);
                upgradeButton.transform.localScale = Vector3.one;
                
                UpgradeButton upgradeButtonScript = upgradeButton.GetComponent<UpgradeButton>();
                upgradeButtonScript.InitButton(upgradeOptions[i]);
                upgradeButtons[i] = upgradeButtonScript.UpgradeBtn;
            }

            InitPathView(towerSO, levelIndex, upgradeHistory);

            return upgradeButtons;
        }

        public void InitPathView(TowerSO towerSO, int levelIndex, int[] upgradeHistory)
        {
            List<GameObject[]> knobs = new List<GameObject[]>();

            //Create Segments
            for (int i = 0; i < towerSO.TowerLevels.Length; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, pathView);
                if (i == levelIndex)
                {
                    segment.GetComponent<Image>().color = currentLevelColor;
                }

                //Clear all knobs
                for (int j = 0; j < segment.transform.childCount; j++)
                {
                    Destroy(segment.transform.GetChild(0));
                }

                //Create Knobs
                GameObject[] buttonList = new GameObject[towerSO.TowerLevels[i].upgradeOptions.Length];
                for (int j = 0; j < towerSO.TowerLevels[i].upgradeOptions.Length; j++)
                {
                    buttonList[j] = Instantiate(segmentKnob, segment.transform);
                }
                knobs.Add(buttonList);
            }

            //Color knobs
            for (int i = 0; i < upgradeHistory.Length; i++)
            {
                knobs[i][upgradeHistory[i]].GetComponent<Image>().color = upgradeBoughtColor;
            }

            RefreshContentSize();
        }

        public void InitSellButton(TowerSO towerSO, int level)
        {
            sellCost.text = $"${towerSO.TowerLevels[level].sellCost}";
        }

        private void RefreshContentSize()
        {
            IEnumerator Routine()
            {
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                yield return null;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            
            StartCoroutine(Routine());
        }

        private void OnDisable()
        {
            Clear();
        }
    }
}