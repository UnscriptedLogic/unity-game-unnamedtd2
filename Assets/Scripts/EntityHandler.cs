using System;
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

    public event EventHandler OnUnitListRemoved;
    public event EventHandler OnTowerListRemoved;

    private void Awake()
    {
        instance = this;

        //TowerBase.OnAnyTowerSpawned += Tower_OnAnyTowerSpawned;
        //TowerBase.OnAnyTowerDespawned += TowerBase_OnAnyTowerDespawned;

        UnitBase.OnAnyUnitSpawned += UnitBase_OnAnyUnitSpawned;
        UnitBase.OnAnyUnitDespawned += UnitBase_OnAnyUnitDespawned;
    }

    private void UnitBase_OnAnyUnitSpawned(object sender, System.EventArgs e) => units.Add(sender as UnitBase);
    private void UnitBase_OnAnyUnitDespawned(object sender, System.EventArgs e)
    {
        units.Remove(sender as UnitBase);
        OnUnitListRemoved?.Invoke(this, EventArgs.Empty);
    }

    private void TowerBase_OnAnyTowerDespawned(object sender, System.EventArgs e) => towers.Remove(sender as TowerBase);
    private void Tower_OnAnyTowerSpawned(object sender, System.EventArgs e)
    {
        towers.Add(sender as TowerBase);
        OnTowerListRemoved?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerator KillAllUnits_Coroutine()
    {
        for (int i = units.Count - 1; i >= 0; i--)
        {
            units[i].KillUnit();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public IEnumerator DisableAllTowers_Coroutine()
    {
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            towers[i].enabled = false;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public IEnumerator KillAllTowers_Coroutine()
    {
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            Destroy(towers[i].gameObject);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
