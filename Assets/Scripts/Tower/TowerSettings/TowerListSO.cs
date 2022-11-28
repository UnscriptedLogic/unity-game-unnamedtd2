using UnityEngine;
using System.Collections.Generic;

namespace TowerManagement
{
    [CreateAssetMenu(fileName = "New Tower List", menuName = "ScriptableObjects/New Tower List", order = 1)]
    public class TowerListSO : ScriptableObject
    {
        [SerializeField] private List<TowerSO> towerList;

        public List<TowerSO> TowerList => towerList;
    }
}