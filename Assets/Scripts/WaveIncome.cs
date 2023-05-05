using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Asynchronous.Timed;
using UnscriptedLogic.MathUtils;

public class WaveIncome : MonoBehaviour
{
    [Header("Income")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private int waveCap;
    [SerializeField] private float multiplier = 100f;
    [SerializeField] private GameSpawnManager spawnManager;

    private void OnEnable()
    {
        spawnManager.OnInitialized += SpawnManager_OnInitialized;
    }

    private void SpawnManager_OnInitialized(object sender, System.EventArgs e)
    {
        spawnManager.WaveSpawner.OnEnterState += WaveSpawner_OnEnterState;
    }

    private void WaveSpawner_OnEnterState(object sender, TimedWaveSpawner.SpawnerState e)
    {
        if (e == TimedWaveSpawner.SpawnerState.SpawningWave)
        {
            TowerDefenseManager.instance.CashSystem.Modify(ModifyType.Add, AddWaveIncome(spawnManager.WaveSpawner.WaveIndex));
        }
    }

    public float AddWaveIncome(int waveCount)
    {
        float evaluation;
        if (waveCount <= waveCap)
            evaluation = waveCount / (float)waveCap;
        else
            evaluation = waveCap;

        float amount = (float)Mathf.Round(animationCurve.Evaluate(evaluation) * multiplier);
        Debug.Log(amount);
        return amount;
    }
}
