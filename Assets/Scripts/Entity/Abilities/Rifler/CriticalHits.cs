using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class CriticalHits : Ability
{
    private float damageMultiplier = 2f;
    private int procChancePercent = 15;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 5, 7, 9 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        tower.ApplyDamage = OverrideDamageMethod;
        tower.DamageHandler.Modify(ModifyType.Add, 1);
        upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));
    }

    public void OverrideDamageMethod(UnitBase unit, float damage)
    {
        if (RandomLogic.IntZeroTo(100) < procChancePercent)
        {
            damage *= damageMultiplier;
        }

        unit.TakeDamage(damage);
    }

    protected override void OnLevelUp()
    {
        if (levelHandler.Current == 2)
        {
            procChancePercent = 25;
            tower.DamageHandler.Modify(ModifyType.Add, 1);
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));

        }
        else if (levelHandler.Current == 3)
        {
            procChancePercent = 30;
            damageMultiplier = 3f;
            tower.DamageHandler.Modify(ModifyType.Add, 1);
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));

        }
        else if (levelHandler.Current == 4)
        {
            procChancePercent = 35;
            damageMultiplier = 4f;
            tower.DamageHandler.Modify(ModifyType.Add, 1);
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));

        }
    }
}
