using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Asynchronous.Timed;

public class GameSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private SpawnSettings spawnSettings;
    [SerializeField] private WaveSO waveSO;

    private TimedWaveSpawner waveSpawner;

    public TimedWaveSpawner WaveSpawner => waveSpawner;

    public event EventHandler OnInitialized;

    private void Start()
    {
        waveSpawner = new TimedWaveSpawner(new SpawnSettings() { startDelay = spawnSettings.startDelay, waves = waveSO.Waves});
        waveSpawner.OnSpawnEnemy += WaveSpawner_OnSpawnEnemy;
        waveSpawner.OnCompleted += WaveSpawner_OnCompleted;

        waveSpawner.Start();
        OnInitialized?.Invoke(this, EventArgs.Empty);
    }

    private void WaveSpawner_OnCompleted(object sender, System.EventArgs e)
    {
        waveSpawner.Stop();

        Debug.Log("All waves completed. Awaiting cleared map.");
        EntityHandler.instance.OnUnitListRemoved += Instance_OnUnitListRemoved;
    }

    private void Instance_OnUnitListRemoved(object sender, System.EventArgs e)
    {
        if (EntityHandler.instance.Units.Count == 0)
        {
            TowerDefenseManager.instance.Win();
        }
    }

    private void Update()
    {
        waveSpawner.Update();
    }

    private void WaveSpawner_OnSpawnEnemy(object sender, OnSpawnArgs e)
    {
        GameObject unit = Instantiate(e.item);
        unit.transform.SetParent(transform);
    }
}
