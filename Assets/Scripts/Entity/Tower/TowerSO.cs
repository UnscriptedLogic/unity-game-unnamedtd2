using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeOption
{
    [Header("Next Upgrade Settings")]
    [SerializeField] private string name;
    [TextArea(3, 10)][SerializeField] private string description;
    [TextArea(3, 10)][SerializeField] private string loreDescription;
    [SerializeField] private Sprite iconSpr;
    [SerializeField] private float cost;
    [SerializeField] private GameObject overrideObject;

    public string Name => name;
    public string Description => description;
    public string LoreDescription => loreDescription;
    public Sprite IconSprite => iconSpr;
    public float Cost => cost;
    public GameObject OverrideObject => overrideObject;
}

[Serializable]
public class TowerLevel
{
    [Header("Level Settings")]
    public string overrideName;
    public float sellCost;
    public Sprite towerIcon;
    public GameObject towerPrefab;

    public UpgradeOption[] upgradeOptions;
}

[CreateAssetMenu(fileName = "New Tower", menuName = "ScriptableObjects/Create New Tower")]
public class TowerSO : ScriptableObject
{
    [Header("Base Settings")]
    [SerializeField] private Sprite iconSpr;
    [SerializeField] private string towerName;
    [SerializeField] private int towerCost;
    [SerializeField] private GameObject towerPrefab;

    [Header("Upgrades")]
    [SerializeField] private TowerLevel[] towerLevels;

    public string TowerName => towerName;
    public Sprite IconSpr => iconSpr;
    public int TowerCost => towerCost;
    public GameObject Prefab => towerPrefab;

    public TowerLevel[] TowerLevels => towerLevels;

    public UpgradeOption[] GetUpgradesAtIndex(int index) => towerLevels[index].upgradeOptions;
}
