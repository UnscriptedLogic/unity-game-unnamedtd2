using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflerUpgradeHandler : TowerUpgradeHandler
{
    private AbilityHandler abilityHandler;
    private AbilityManager abilityManager;

    protected override void InitUpgrades(Tower towerBase)
    {
        base.InitUpgrades(towerBase);

        abilityHandler = towerBase.GetComponent<AbilityHandler>();
        abilityManager = AbilityManager.instance;

        //Level 1
        UpgradeGroup level1 = new UpgradeGroup();
        level1.upgradeProperties.Add(new UpgradeProperty(reloadTime: -1));
        level1.upgradeProperties.Add(new UpgradeProperty(damage: 1));
        upgradeGroups.Add(level1);

        //Level 2
        UpgradeGroup level2 = new UpgradeGroup();
        level2.upgradeProperties.Add(new UpgradeProperty(method: (tower) =>
        {
            abilityHandler.AddAbility(new CriticalHits(), tower);
        }));

        level2.upgradeProperties.Add(new UpgradeProperty(reloadTime: -0.25f));
        upgradeGroups.Add(level2);

        //Level 3
        UpgradeGroup level3 = new UpgradeGroup();
        level3.upgradeProperties.Add(new UpgradeProperty(method: (tower) =>
        {
            abilityHandler.AddAbility(new RelentlessStacks(), tower);
        }));

        level3.upgradeProperties.Add(new UpgradeProperty(projPierce: 5));
        upgradeGroups.Add(level3);
    }
}