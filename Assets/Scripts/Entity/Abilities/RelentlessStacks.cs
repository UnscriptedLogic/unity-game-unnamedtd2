using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;

public class RelentlessStacks : Ability
{
    //private float intervalDecrease = 0.1f;
    private float maxStack = 5;
    private float currentStack = 0;

    private GameObject unit;
     
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[maxLevel];
        levelRequirements[0] = 1;
        levelRequirements[1] = 3;
        levelRequirements[2] = 4;
        levelRequirements[3] = 7;

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        currentStack = 0;
    }

    protected override void OnProjectileHit(object sender, OnProjectileHitEventArgs e)
    {
        if (unit == null)
        {
            unit = e.unit.gameObject;
            return;
        }

        if (unit.GetInstanceID() == e.unit.gameObject.GetInstanceID())
        {
            if (currentStack < maxStack)
            {
                currentStack++;
                //tower.ReloadTime -= intervalDecrease;
            }
            
            return;
        }

        unit = null;
        //tower.ReloadTime += currentStack * intervalDecrease;
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
