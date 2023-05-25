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

        extraDamagesPerMeter = new int[4] { 50, 75, 100, 150 };

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
            damage += Mathf.RoundToInt(distance * extraDamage);

            (GameObject sound, GameObject effect) = fxManager.PlayFXPair(fxManager.GlobalEffects.Crit, unit.transform.position + Vector3.up, UnityEngine.Quaternion.identity, Vector3.one);
            effect.GetComponent<EffectUI>().AmountTMP.text = damage.ToString();
        }

        unit.TakeDamage(damage);
    }
}
