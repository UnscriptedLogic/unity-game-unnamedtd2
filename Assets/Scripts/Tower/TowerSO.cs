using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "ScriptableObjects/Create New Tower")]
public class TowerSO : ScriptableObject
{
    [SerializeField] private string towerName;
    [SerializeField] private Sprite iconSpr;
    [SerializeField] private int towerCost;
    [SerializeField] private GameObject towerPrefab;

    public string TowerName => towerName;
    public Sprite IconSpr => iconSpr;
    public int TowerCost => towerCost;
    public GameObject Prefab => towerPrefab;
}
