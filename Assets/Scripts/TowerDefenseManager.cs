using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;

public class TowerDefenseManager : MonoBehaviour
{
    [SerializeField] private int startHealth;
    [SerializeField] private float startCash;

    private CurrencyHandler healthSystem;
    private CurrencyHandler cashSystem;

    public CurrencyHandler HealthSystem => healthSystem;
    public CurrencyHandler CashSystem => cashSystem;

    private void Start()
    {
        healthSystem = new CurrencyHandler(startHealth);
        cashSystem = new CurrencyHandler(startCash);

        UnitBase.OnAnyUnitCompletedPath += OnUnitCompletedPath;
    }

    private void OnUnitCompletedPath(object sender, EventArgs eventArgs)
    {
        UnitBase unitScript = sender as UnitBase;

        Destroy(unitScript.gameObject);
    }
}
