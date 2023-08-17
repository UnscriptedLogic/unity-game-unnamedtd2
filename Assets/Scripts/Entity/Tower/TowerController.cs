using System;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Builders;

public class TowerController : MonoBehaviour, IBuildable, IInspectable
{
    [SerializeField] private string id;
    [SerializeField] private TowerStatsHandler statHandler;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    public TowerEvents Events { get; private set; }
    public TowerStats Stats { get; private set; }

    public string ID => id;
    public SkinnedMeshRenderer SkinnedMeshRenderer => meshRenderer;

    public event EventHandler OnControllerInitialized;
    public event EventHandler OnTowerInitialized;

    public virtual void LocalPassBuildConditions<T>(T builder, out List<LocalBuildCondition> localBuildConditions)
    {
        BuildManager buildManager = builder as BuildManager;
        localBuildConditions = new List<LocalBuildCondition>()
        {
            new LocalBuildCondition("Test", (pos, rot) => true, "Test Failed", "Test Succeeded"),
            //new LocalBuildCondition("Cost", (pos, rot) => tdManager.AllTowerList.GetSOFromTower(this).TowerCost <= tdManager.CurrentCash, "Insufficient Cash", "Sufficient Cash")
        };
    }

    private void Start()
    {
        //Pre requisites
        Events = new TowerEvents();
        Stats = statHandler.CalculatedTowerStats;

        OnControllerInitialized?.Invoke(this, EventArgs.Empty);

        //Other stuff

        OnTowerInitialized?.Invoke(this, EventArgs.Empty);
    }
}
