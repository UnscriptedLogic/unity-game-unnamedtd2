using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace TowerManagement
{
    public class RapidTower : TowerUpgradeHandler
    {
        protected override void InitUpgrades()
        {
            UpgradeGroup level1 = new UpgradeGroup();
            level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.05f));
            level1.upgradeProperties.Add(new UpgradeProperty(damage: 2));
            level1.upgradeProperties.Add(new UpgradeProperty(range: 5));
            upgradeGroups.Add(level1);

            UpgradeGroup level2 = new UpgradeGroup();
            level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.025f));
            level2.upgradeProperties.Add(new UpgradeProperty(damage: 5));
            level2.upgradeProperties.Add(new UpgradeProperty(damage: 5));
            upgradeGroups.Add(level2);

            UpgradeGroup level3 = new UpgradeGroup();
            level3.upgradeProperties.Add(new UpgradeProperty(projLifetime: 2f));
            level3.upgradeProperties.Add(new UpgradeProperty(projSpeed: 10));
            level3.upgradeProperties.Add(new UpgradeProperty(damage: 5));
            upgradeGroups.Add(level3);

            //UpgradeGroup level4 = new UpgradeGroup();
            //level4.upgradeProperties.Add(new UpgradeProperty(projLifetime: 2f));
            //level4.upgradeProperties.Add(new UpgradeProperty(projSpeed: 10));
            //level4.upgradeProperties.Add(new UpgradeProperty(damage: 5));
            //upgradeGroups.Add(level4);
        }
    }
}