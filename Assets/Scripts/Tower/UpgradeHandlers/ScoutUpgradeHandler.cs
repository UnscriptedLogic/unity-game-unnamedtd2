using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
    public class ScoutUpgradeHandler : TowerUpgradeHandler
    {
        private bool useDouble;

        protected override void InitUpgrades(TowerBase towerBase)
        {
            UpgradeGroup level1 = new UpgradeGroup();
            level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
            level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.5f));
            level1.upgradeProperties.Add(new UpgradeProperty(projPierce: 1));
            upgradeGroups.Add(level1);

            UpgradeGroup level2 = new UpgradeGroup();
            level2.upgradeProperties.Add(new UpgradeProperty(damage: 1));
            level2.upgradeProperties.Add(new UpgradeProperty(range: 2));
            level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.5f));
            upgradeGroups.Add(level2);

            UpgradeGroup level3 = new UpgradeGroup();
            level3.upgradeProperties.Add(new UpgradeProperty(damage: 1));
            level3.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.2f, method: towerBase =>
            {
                useDouble = true;
                ScoutTower scout = (ScoutTower)towerBase;
                scout.useDoubleShells = true;
            }));

            level3.upgradeProperties.Add(new UpgradeProperty());
            upgradeGroups.Add(level3);

            UpgradeGroup level4 = new UpgradeGroup();
            level4.upgradeProperties.Add(new UpgradeProperty(projPierce: 5, method: towerBase =>
            {
                ScoutTower scout = (ScoutTower)towerBase;
                scout.projectileBehaviour = new RicochetProjectile(3f);
            }));
            
            level4.upgradeProperties.Add(new UpgradeProperty(damage: 5, projSpeed: 5, method: towerBase =>
            {
                ScoutTower scout = (ScoutTower)towerBase;

                if (useDouble)
                {
                    scout.useQuadShells = true;
                    return;
                }

                scout.useDoubleShells = true;
            }));
            
            level4.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.1f));
            upgradeGroups.Add(level4);
        }
    }
}