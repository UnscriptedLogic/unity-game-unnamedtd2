using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Infinite;

public class SpawnManager : MonoBehaviour
{
    private PointBasedWaveSystem waveSystem;
    [SerializeField] private SpawnerSettings spawnerSettings;

    private void Start()
    {
        waveSystem = new PointBasedWaveSystem(spawnerSettings, OnSpawn, OnCompleted, true);

        TowerDefenseManager.instance.OnInitialized += Instance_OnInitialized;
    }

    private void Instance_OnInitialized(object sender, EventArgs e)
    {
        TowerDefenseManager.instance.HealthSystem.OnEmpty += HealthSystem_OnEmpty;
    }

    private void HealthSystem_OnEmpty(object sender, EventArgs e)
    {
        waveSystem.Pause();
        EntityHandler.instance.KillAllUnits();
    }

    private void Update()
    {
        waveSystem.UpdateSpawner();
    }

    private void OnSpawn(GameObject obj)
    {
        Instantiate(obj);
    }

    private void OnCompleted()
    {

    }
}
