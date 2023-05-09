using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Asynchronous.Timed;

public class WaveTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTMP;
    [SerializeField] private GameSpawnManager spawnManager;

    private void Start()
    {
        spawnManager.OnInitialized += SpawnManager_OnInitialized;
    }

    private void SpawnManager_OnInitialized(object sender, System.EventArgs e)
    {
        spawnManager.WaveSpawner.OnEnterState += WaveSpawner_OnEnterState;
    }

    private void Update()
    {
        switch (spawnManager.WaveSpawner.CurrentState)
        {
            case TimedWaveSpawner.SpawnerState.Starting:
                waveTMP.text = $"Starting in {Math.Round(spawnManager.WaveSpawner.Interval, 1)}";
                break;
            case TimedWaveSpawner.SpawnerState.Waiting:
                if (spawnManager.WaveSpawner.WaveIndex < spawnManager.WaveSpawner.WaveAmount - 1)
                {
                    if (spawnManager.WaveSpawner.Interval <= 5f)
                    {
                        waveTMP.text = $"Next Wave in {Math.Round(spawnManager.WaveSpawner.Interval, 1)}";
                    } 
                } else
                {
                    waveTMP.text = $"Clear all enemies to finish the level";
                }
                break;
            default:
                break;
        }
    }

    private void WaveSpawner_OnEnterState(object sender, TimedWaveSpawner.SpawnerState e)
    {
        switch (e)
        {
            case TimedWaveSpawner.SpawnerState.SpawningWave:
                waveTMP.text = $"Wave: {spawnManager.WaveSpawner.WaveIndex + 1}";
                break;
        }
    }
}
