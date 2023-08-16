using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class CriticalHits : Ability
{
    private float damageMultiplier = 2f;
    private int procChancePercent = 15;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 5, 10, 15 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        tower.ApplyDamage = OverrideDamageMethod;
        upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));
    }

    public void OverrideDamageMethod(UnitBase unit, float damage)
    {
        //if (true)
        if (RandomLogic.IntZeroTo(100) < procChancePercent)
        {
            damage *= damageMultiplier;
            (GameObject sound, GameObject effect) = fxManager.PlayFXPair(fxManager.GlobalEffects.Crit, unit.transform.position + Vector3.up, UnityEngine.Quaternion.identity, Vector3.one);
            effect.GetComponent<EffectUI>().AmountTMP.text = damage.ToString();
        }

        unit.TakeDamage(damage);
    }

    protected override void OnLevelUp()
    {
        if (levelHandler.Current == 2)
        {
            procChancePercent = 25;
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));
        }
        else if (levelHandler.Current == 3)
        {
            procChancePercent = 30;
            damageMultiplier = 3f;
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));

        }
        else if (levelHandler.Current == 4)
        {
            procChancePercent = 35;
            damageMultiplier = 4f;
            upgradeHandler.UpdatePersistentStats(new TowerUpgradeHandler.UpgradeProperty(damage: 1));

        }
    }
}
