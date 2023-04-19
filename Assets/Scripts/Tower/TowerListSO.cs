using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Builders;

[CreateAssetMenu(fileName = "New Tower List", menuName = "ScriptableObjects/Tower List")]
public class TowerListSO : ScriptableObject
{
    [SerializeField] private List<TowerSO> towerList;
    public List<TowerSO> TowerList => towerList;
}
