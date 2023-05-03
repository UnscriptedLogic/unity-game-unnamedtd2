using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Classic;

public class GameSpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnSettings spawnSettings;
    private SimpleWaveSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = new SimpleWaveSpawner(spawnSettings);
        waveSpawner.OnSpawnEnemy += WaveSpawner_OnSpawnEnemy;
        waveSpawner.OnCompleted += WaveSpawner_OnCompleted;

        waveSpawner.Start();
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
