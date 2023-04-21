using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflerUpgradeHandler : TowerUpgradeHandler
{
    protected override void InitUpgrades(Tower towerBase)
    {
        UpgradeGroup level1 = new UpgradeGroup();
        level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -1));
        level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
        upgradeGroups.Add(level1);

        UpgradeGroup level2 = new UpgradeGroup();
        level2.upgradeProperties.Add(new UpgradeProperty(damage: 1, projPierce: 1));
        level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.25f));
        upgradeGroups.Add(level2);
    }
}