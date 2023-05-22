using System;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;

public class OnAnyUnitAbilityEventArgs : EventArgs
{
    public UnitBase unit;
}

public struct UnitAbilityInitSettings
{
    public UnitAbilityHandler abilityHandler;
    public UnitBase unit;
    public FXManager fxManager;
}

public class UnitAbility
{
    protected FXManager fxManager;
    protected UnitAbilityHandler abilityHandler;
    protected CurrencyHandler levelHandler;
    protected UnitBase unit;

    protected int maxLevel;
    protected int[] levelRequirements;

    public void Initialize(UnitAbilityInitSettings initSettings)
    {
        abilityHandler = initSettings.abilityHandler;
        unit = initSettings.unit;
        fxManager = initSettings.fxManager;

        OnAdded();
    }

    public virtual void OnAdded() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}

public class UnitAbilityHandler : MonoBehaviour
{
    [SerializeField] private List<UnitAbility> abilities;

    private FXManager fXManager;
    private UnitBase unit;

    public List<UnitAbility> Abilities => abilities;

    private void Start()
    {
        abilities = new List<UnitAbility>();
        fXManager = FXManager.instance;
        unit = transform.GetComponent<UnitBase>();
    }

    public void AddAbility(UnitAbility ability, UnitBase unit)
    {
        UnitAbilityInitSettings settings = new UnitAbilityInitSettings()
        {
            abilityHandler = this,
            unit = unit,
            fxManager = FXManager.instance
        };

        ability.Initialize(settings);
        abilities.Add(ability);
    }

    private void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].Update();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].FixedUpdate();
        }
    }
}
