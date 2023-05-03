using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUpgradeHandler : TowerUpgradeHandler
{
    private AbilityHandler abilityHandler;
    private AbilityManager abilityManager;
    private ArcherTower archerTower;

    protected override void InitUpgrades(TowerBase towerBase)
    {
        base.InitUpgrades(towerBase);

        abilityHandler = towerBase.GetComponent<AbilityHandler>();
        abilityManager = AbilityManager.instance;
        archerTower = towerBase as ArcherTower;

        //Level 1
        UpgradeGroup level1 = new UpgradeGroup();
        level1.upgradeProperties.Add(new UpgradeProperty(damage: 3));
        level1.upgradeProperties.Add(new UpgradeProperty(method: tower =>
        {
            abilityHandler.AddAbility(new PowerShot(), tower);
        }));
        upgradeGroups.Add(level1);

        //Level 2
        UpgradeGroup level2 = new UpgradeGroup();
        level2.upgradeProperties.Add(new UpgradeProperty(range: 3, projSpeed: 5));
        level2.upgradeProperties.Add(new UpgradeProperty(method: tower =>
        {
            abilityHandler.AddAbility(new ProjectileMaintenance(), tower);
        }));
        upgradeGroups.Add(level2);

        //Level 3
        UpgradeGroup level3 = new UpgradeGroup();
        level3.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.5f));
        level3.upgradeProperties.Add(new UpgradeProperty(method: tower =>
        {
            abilityHandler.AddAbility(new SplitShot(), tower);
        }));
        upgradeGroups.Add(level3);

        //Level 4
        UpgradeGroup level4 = new UpgradeGroup();
        level4.upgradeProperties.Add(new UpgradeProperty(damage: 6, projSpeed: 5));
        level4.upgradeProperties.Add(new UpgradeProperty(method: tower =>
        {
            abilityHandler.AddAbility(new TippedArrows(), tower);
        }));
        upgradeGroups.Add(level4);
    }
}
