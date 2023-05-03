using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;

public class PowerShot : Ability
{
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[maxLevel];
        levelRequirements[0] = 1;
        levelRequirements[1] = 2;
        levelRequirements[2] = 5;
        levelRequirements[3] = 7;

        levelHandler = new CurrencyHandler(1, max: maxLevel);
    }
}
