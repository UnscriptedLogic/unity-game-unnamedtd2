using System.Collections;
using System.Collections.Generic;
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

        levelRequirements = new int[maxLevel];
        levelRequirements[0] = 1;
        levelRequirements[1] = 2;
        levelRequirements[2] = 5;
        levelRequirements[3] = 7;

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        tower.ApplyDamage = OverrideDamageMethod;
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
        }
        else if (levelHandler.Current == 3)
        {
            procChancePercent = 30;
            damageMultiplier = 3f;
        }
        else if (levelHandler.Current == 4)
        {
            procChancePercent = 35;
            damageMultiplier = 4f;
        }
    }
}
