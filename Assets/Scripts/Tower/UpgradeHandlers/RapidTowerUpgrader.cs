using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace TowerManagement
{
    public class RapidTowerUpgrader : TowerUpgradeHandler
    {
        protected override void InitUpgrades(TowerBase towerBase)
        {
            UpgradeGroup level1 = new UpgradeGroup();
            level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.025f));
            level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
            level1.upgradeProperties.Add(new UpgradeProperty(range: 2));
            upgradeGroups.Add(level1);

            UpgradeGroup level2 = new UpgradeGroup();
            level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.025f));
            level2.upgradeProperties.Add(new UpgradeProperty(damage: 5));
            level2.upgradeProperties.Add(new UpgradeProperty(projPierce: 2));
            upgradeGroups.Add(level2);

            UpgradeGroup level3 = new UpgradeGroup();
            level3.upgradeProperties.Add(new UpgradeProperty(projLifetime: 2f));
            level3.upgradeProperties.Add(new UpgradeProperty(projSpeed: 10));
            level3.upgradeProperties.Add(new UpgradeProperty(damage: 2, projPierce: 2));
            upgradeGroups.Add(level3);

            UpgradeGroup level4 = new UpgradeGroup();
            level4.upgradeProperties.Add(new UpgradeProperty(method: towerbase => 
            {
                RapidTower rapidTower = (RapidTower)towerbase;
                rapidTower.useTriple = true; 
                Debug.Log($"Triple mode: {rapidTower.useTriple}"); 
            }));
            
            level4.upgradeProperties.Add(new UpgradeProperty(projPierce: 6));
            level4.upgradeProperties.Add(new UpgradeProperty());
            upgradeGroups.Add(level4);
        }
    }
}