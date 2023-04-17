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
