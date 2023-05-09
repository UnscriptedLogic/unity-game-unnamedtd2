using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public class StartManager : MonoBehaviour
{
    private void Start()
    {
        UnitBase.OnAnyUnitCompletedPath += OnUnitCompletedPath;
    }

    private void OnUnitCompletedPath(object sender, EventArgs eventArgs)
    {
        GameObject unit = sender as GameObject;
        Destroy(unit);
    }
}
