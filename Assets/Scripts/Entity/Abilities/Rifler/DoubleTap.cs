using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class DoubleTap : Ability
{
    private float quickReloadTime = 0.25f;
    private int procChancePercent = 10;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[maxLevel];
        levelRequirements[0] = 1;
        levelRequirements[1] = 2;
        levelRequirements[2] = 6;
        levelRequirements[3] = 10;

        levelHandler = new CurrencyHandler(1, max: maxLevel);
    }

    protected override void OnProjectileFired()
    {
        if (RandomLogic.IntZeroTo(100) < procChancePercent)
        {
            tower.CurrentReloadtime = quickReloadTime;
        }
    }

    protected override void OnLevelUp()
    {
        if (levelHandler.Current == 2)
        {
            procChancePercent = 14;
        }
        else if (levelHandler.Current == 3)
        {
            procChancePercent = 19;
        }
        else if (levelHandler.Current == 4)
        {
            procChancePercent = 25;
        }
    }
}