using System;
using UnityEngine;

namespace TowerManagement
{
    public enum TowerPlacement
    {
        Ground,
        Water,
        Path
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/New Tower", fileName = "New Tower", order = 0)]
    public class TowerSO : ScriptableObject
    {
        [Serializable]
        public class UpgradeOption 
        {
            [Header("Next Upgrade Settings")]
            public Sprite nextUpgradeIcon;
            public float nextUpgradeCost;
            [TextArea(3, 6)] public string nextUpgradeDesc;
            public GameObject overrideObject;
        }

        [Serializable]
        public class TowerLevel
        {
            [Header("Level Settings")]
            public string overrideName;
            public float sellCost;
            public Sprite towerIcon;
            public GameObject towerPrefab;
            public GameObject towerConstruct;

            public UpgradeOption[] upgradeOptions;
        }

        [SerializeField] private Sprite icon;
        [SerializeField] private Color bgColor;
        [SerializeField] private Color brColor;
        [SerializeField] private string towerName;
        [SerializeField] private float cost;
        [SerializeField] private TowerPlacement towerPlacement;
        [TextArea(5, 10)][SerializeField] private string towerDescription;

        [SerializeField] private TowerLevel[] towerLevels;

        public Sprite Icon => icon;
        public Color BGColor => bgColor;
        public Color BRColor => brColor;
        public string TowerName => towerName;
        public float Cost => cost;
        public string TowerDescription => towerDescription;
        public TowerPlacement TowerPlacement => towerPlacement;
        public TowerLevel[] TowerLevels => towerLevels;

        private void OnValidate()
        {
            for (int i = 0; i < towerLevels.Length; i++)
            {
                if (towerLevels[i].overrideName == "")
                {
                    towerLevels[i].overrideName = towerName;
                }

                if (towerLevels[i].towerIcon == null)
                {
                    towerLevels[i].towerIcon = icon;
                }

                if (i == 0)
                {
                    towerLevels[i].sellCost = cost * 0.5f;
                } else
                {
                    if (towerLevels[i].upgradeOptions.Length <= 0) return;
                    
                    float lowest = 9999999f;
                    for (int j = 0; j < towerLevels[i - 1].upgradeOptions.Length; j++)
                    {
                        lowest = Mathf.Min(lowest, towerLevels[i - 1].upgradeOptions[j].nextUpgradeCost);
                    }

                    towerLevels[i].sellCost = Mathf.RoundToInt(towerLevels[i - 1].sellCost + (lowest * 0.5f));
                }
            }
        }
    }
}