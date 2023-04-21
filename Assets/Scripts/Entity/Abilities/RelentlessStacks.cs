using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;

public class RelentlessStacks : Ability
{
    private float intervalDecrease = 0.05f;
    private float maxStack = 5;
    private float currentStack = 0;

    private GameObject unit;
     
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[maxLevel];
        levelRequirements[0] = 5;
        levelRequirements[1] = 10;
        levelRequirements[2] = 15;
        levelRequirements[3] = 25;

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        currentStack = 0;
    }

    protected override void OnProjectileHit(UnitBase unitScript, ProjectileBase projectileScript, Action<UnitBase, float> damageMethod)
    {
        if (unit == null)
        {
            unit = unitScript.gameObject;
            tower.ReloadTime += currentStack * intervalDecrease;
            currentStack= 0;
        }

        if (unit == unitScript.gameObject)
        {
            if (currentStack < maxStack)
            {
                currentStack++;
                tower.ReloadTime -= intervalDecrease;
            }
            
            return;
        } 

        tower.ReloadTime += currentStack * intervalDecrease;
        currentStack = 0;
    }

    protected override void OnLevelUp()
    {
        if (levelHandler.Current == 2)
        {
            maxStack = 7;
        }
        else if (levelHandler.Current == 3)
        {
            maxStack = 12;
        }
        else if (levelHandler.Current == 4)
        {
            maxStack = 15;
        }
    }
}
