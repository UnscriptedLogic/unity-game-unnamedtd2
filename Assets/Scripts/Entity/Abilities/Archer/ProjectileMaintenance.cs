using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class ProjectileMaintenance : Ability
{
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 5, 8, 12 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        OnLevelUp();
    }

    protected override void OnLevelUp()
    {
        upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(projPierce: 2, reloadTime: -0.5f));
    }
}
