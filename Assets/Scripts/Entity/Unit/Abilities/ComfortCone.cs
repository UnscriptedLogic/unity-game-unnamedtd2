using System;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class ComfortCone : UnitAbility
{
    private bool called;
    private int damageReductionPercent = 30;

    public override void OnAdded()
    {
        unit.HealthHandler.OnModified += HealthHandler_OnModified;
        unit.OverrideApplyDamage += OverrideApplyDamage;
    }

    private void HealthHandler_OnModified(object sender, CurrencyEventArgs e)
    {
        if (called) return;

        if (e.modifyType == ModifyType.Subtract)
        {
            if (unit.HealthHandler.Current <= unit.MaxHealth * 0.5f)
            {
                called = true;
            }
        }
    }

    public void OverrideApplyDamage(float damage)
    {
        if (!called)
        {
            damage = damage / 100f * (100 - damageReductionPercent);
        }

        unit.HealthHandler.Modify(ModifyType.Subtract, damage);
    }
}
