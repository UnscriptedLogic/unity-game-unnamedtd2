using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflerUpgradeHandler : TowerUpgradeHandler
{
    private AbilityHandler abilityHandler;
    private AbilityManager abilityManager;
    private RiflerTower riflerTower;

    protected override void InitUpgrades(TowerBase towerBase)
    {
        base.InitUpgrades(towerBase);

        abilityHandler = towerBase.GetComponent<AbilityHandler>();
        abilityManager = AbilityManager.instance;
        //riflerTower = towerBase as RiflerTower;

        //Level 1
        UpgradeGroup level1 = new UpgradeGroup();
        level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.5f));
        level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
        upgradeGroups.Add(level1);

        //Level 2
        UpgradeGroup level2 = new UpgradeGroup();
        level2.upgradeProperties.Add(new UpgradeProperty(method: (tower) =>
        {
            abilityHandler.AddAbility(new CriticalHits(), tower);
        }));

        level2.upgradeProperties.Add(new UpgradeProperty(range: 2f));
        upgradeGroups.Add(level2);

        //Level 3
        UpgradeGroup level3 = new UpgradeGroup();
        level3.upgradeProperties.Add(new UpgradeProperty(method: (tower) =>
        {
            abilityHandler.AddAbility(new DoubleTap(), tower);
        }));

        level3.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.25f));
        upgradeGroups.Add(level3);

        //Level 4
        UpgradeGroup level4 = new UpgradeGroup();
        level4.upgradeProperties.Add(new UpgradeProperty(projPierce: 2));
        level4.upgradeProperties.Add(new UpgradeProperty(method: tower =>
        {
            riflerTower.useHitscan = true;
        }));
        upgradeGroups.Add(level4);
    }
}