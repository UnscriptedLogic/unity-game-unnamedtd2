using UnityEngine;
using UnscriptedLogic.Currency;

public class TippedArrows : Ability
{
    private int currentShot;
    private int shotsBeforeNext = 4;
    private int[] extraDamagesPerMeter;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 10, 15, 20 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        extraDamagesPerMeter = new int[4] { 25, 50, 75, 100 };

        currentShot = 0;

        tower.ApplyDamage = DistanceBasedDamage;
    }

    protected override void OnProjectileFired()
    {
        if (currentShot >= shotsBeforeNext)
        {
            currentShot = 0;
        }

        currentShot++;
    }

    private void DistanceBasedDamage(UnitBase unit, float damageToApply)
    {
        float damage = damageToApply;
        if (currentShot >= shotsBeforeNext)
        {
            //Distance based shot
            float distance = Vector3.Distance(tower.transform.position, unit.transform.position);
            float extraDamage = tower.Damage / 100 * extraDamagesPerMeter[CurrentLevel - 1];
            damage += distance * extraDamage;
        }

        unit.TakeDamage(damage);
    }
}
