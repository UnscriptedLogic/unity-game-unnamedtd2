using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    public static EntityHandler instance { get; private set; }

    private List<TowerBase> towers = new List<TowerBase>();
    private List<UnitBase> units = new List<UnitBase>();

    public List<TowerBase> Towers => towers;
    public List<UnitBase> Units => units;

    private void Awake()
    {
        instance = this;

        TowerBase.OnAnyTowerSpawned += Tower_OnAnyTowerSpawned;
        TowerBase.OnAnyTowerDespawned += TowerBase_OnAnyTowerDespawned;

        UnitBase.OnAnyUnitSpawned += UnitBase_OnAnyUnitSpawned;
        UnitBase.OnAnyUnitDespawned += UnitBase_OnAnyUnitDespawned;
    }

    private void UnitBase_OnAnyUnitSpawned(object sender, System.EventArgs e) => units.Remove(sender as UnitBase);
    private void UnitBase_OnAnyUnitDespawned(object sender, System.EventArgs e) => units.Remove(sender as UnitBase);

    private void TowerBase_OnAnyTowerDespawned(object sender, System.EventArgs e) => towers.Remove(sender as TowerBase);
    private void Tower_OnAnyTowerSpawned(object sender, System.EventArgs e) => towers.Add(sender as TowerBase);

    public void KillAllUnits()
    {
        for (int i = units.Count - 1; i >= 0; i--)
        {
            units[i].KillUnit();
        }
    }

    public void DisableAllTowers()
    {
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            towers[i].enabled = false;
        }
    }
}
