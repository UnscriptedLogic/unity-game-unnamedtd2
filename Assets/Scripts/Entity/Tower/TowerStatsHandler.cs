using System;
using UnityEngine;
using System.Collections.Generic;

public class TowerStatsHandler : TowerComponent
{
    [SerializeField] private TowerStats baseStats;

    private List<ITowerStatModifier> statModifiers;

    public TowerStats CalculatedTowerStats { get; private set; }

    public event EventHandler<TowerStats> OnTowerStatsChanged;

    private void Start()
    {
        statModifiers = new List<ITowerStatModifier>();
        OnTowerStatsChanged += OnStatChanged;
    }

    public void AddModifier(ITowerStatModifier towerStatModifier)
    {
        statModifiers.Add(towerStatModifier);
        OnTowerStatsChanged?.Invoke(this, CalculatedTowerStats);

    }

    public void RemoveModifier(ITowerStatModifier towerStatModifier)
    {
        statModifiers.Remove(towerStatModifier);
        OnTowerStatsChanged?.Invoke(this, CalculatedTowerStats);
    }

    private void OnStatChanged(object sender, TowerStats currentStats)
    {
        CalculateStats();
    }

    private TowerStats CalculateStats()
    {
        TowerStats calculatedStats = baseStats;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            calculatedStats = statModifiers[i].ModifyStats(calculatedStats);
        }

        CalculatedTowerStats = calculatedStats;
        return calculatedStats;
    }
}
