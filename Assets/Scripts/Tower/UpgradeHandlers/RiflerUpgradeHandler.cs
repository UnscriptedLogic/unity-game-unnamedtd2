using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflerUpgradeHandler : TowerUpgradeHandler
{
    private bool useDouble;

    protected override void InitUpgrades(Tower towerBase)
    {
        UpgradeGroup level1 = new UpgradeGroup();
        level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
        level1.upgradeProperties.Add(new UpgradeProperty(projPierce: 1));
        upgradeGroups.Add(level1);

        UpgradeGroup level2 = new UpgradeGroup();
        level2.upgradeProperties.Add(new UpgradeProperty(damage: 1, projPierce: 1));
        level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.25f));
        upgradeGroups.Add(level2);

        UpgradeGroup level3 = new UpgradeGroup();
        level3.upgradeProperties.Add(new UpgradeProperty(damage: 1, method: towerBase =>
        {
            RiflerTower tower = towerBase as RiflerTower;
            tower.useShrapnel = true;
        }));

        level3.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.1f, method: towerBase =>
        {
            useDouble = true;
            RiflerTower scout = (RiflerTower)towerBase;
            scout.useDoubleShells = true;
        }));

        upgradeGroups.Add(level3);

        UpgradeGroup level4 = new UpgradeGroup();
        level4.upgradeProperties.Add(new UpgradeProperty(projPierce: 3, method: towerBase =>
        {
            RiflerTower scout = (RiflerTower)towerBase;
            scout.projectileBehaviour = new RicochetProjectile(3f);
        }));

        level4.upgradeProperties.Add(new UpgradeProperty(method: towerBase =>
        {
            RiflerTower scout = (RiflerTower)towerBase;

            if (useDouble)
            {
                scout.useQuadShells = true;
                scout.Damage += 2;
                scout.ProjectileSpeed += 5;
                return;
            }

            scout.useDoubleShells = true;
        }));

        level4.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.25f));
        upgradeGroups.Add(level4);
    }
}