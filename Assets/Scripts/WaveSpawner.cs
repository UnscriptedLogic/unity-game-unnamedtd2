using System;
using UnityEngine;
using UnityEngine.Events;
using GridManagement;
using GameManagement;
using UnitManagement;
using TMPro;

namespace Game.Spawning
{
    public class WaveSpawner : MonoBehaviour, IModifiesCurrency
    {
        private enum SpawnerStates
        {
            Stopped,
            SpawningWave,
            SpawningSegment,
            Waiting,
            Preparation,
            FinalWait
        }

        [SerializeField] private WavesSO wavesSO;
        [SerializeField] private float baseHealth = 150f;
        [SerializeField] private float startDelay = 5f;
        [SerializeField] private int waveIndex;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Transform cam;

        private Wave currWave;
        private WaveSegment currSegment;
        private SpawnerStates currentState = SpawnerStates.Stopped;
        private GridNode[] nodePath;

        private float _interval;
        private int _spawnAmount;

        private int segmentIndex;
        private int waveCount;
        private bool stopSpawning = true;
        
        private CurrencyManager currencyManager;
        [SerializeField] private TextMeshProUGUI healthTMP;
        [SerializeField] private TextMeshProUGUI waveCounterTMP;

        public int WaveCount => waveCount + 1;
        public WavesSO WavesSO => wavesSO;
        public Action<int, int> OnWaveCompleted;
        public Action<int, int> OnWaveStarted;
        public Action OnCompleted;
        public UnityEvent<float> OnUnitHealthDeducted;
        public Action OnBaseHealthDepleted;

        public void Initialize(GridNode[] path)
        {
            nodePath = path;
        }

        public void StartSpawner()
        {
            stopSpawning = false;
            currWave = wavesSO.Waves[waveIndex];
            currSegment = currWave.waveSegments[segmentIndex];
            waveCount = 0;

            SwitchState(SpawnerStates.Preparation);
            OnWaveStarted += (current, total) => UpdateWaveUI();
            UpdateWaveUI();
            healthTMP.text = baseHealth.ToString();
        }

        private void Update()
        {
            if (stopSpawning)
                return;

            UpdateState();
        }

        private void EnterState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    currWave = wavesSO.Waves[waveIndex];
                    break;
                case SpawnerStates.SpawningSegment:
                    OnWaveStarted?.Invoke(waveIndex, wavesSO.Waves.Length - 1);
                    currSegment = wavesSO.Waves[waveIndex].waveSegments[segmentIndex];
                    _spawnAmount = 0;
                    break;
                case SpawnerStates.Waiting:
                    _interval = currSegment.segmentInterval;
                    break;
                case SpawnerStates.Preparation:
                    _interval = startDelay;
                    break;
                case SpawnerStates.FinalWait:
                    break;
                default:
                    break;
            }
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    if (_interval <= 0f)
                    {
                        SwitchState(SpawnerStates.SpawningSegment);
                        break;
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                        if (transform.childCount == 0 && _interval > 2f)
                        {
                            _interval = 2f;
                        }
                    }
                    break;
                case SpawnerStates.SpawningSegment:
                    if (_spawnAmount >= currSegment.amount)
                    {
                        SwitchState(SpawnerStates.Waiting);
                        break;
                    }

                    if (_interval <= 0f)
                    {
                        SpawnEnemy();
                        _spawnAmount++;
                        _interval = currSegment.interval;
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.Waiting:
                    if (_interval <= 0f)
                    {
                        segmentIndex++;
                        if (segmentIndex >= wavesSO.Waves[waveIndex].waveSegments.Length)
                        {
                            segmentIndex = 0;
                            waveIndex++;
                            waveCount++;
                            OnWaveCompleted?.Invoke(waveIndex, wavesSO.Waves.Length - 1);
                            if (waveIndex >= wavesSO.Waves.Length)
                            {
                                SpawningCompleted();
                                break;
                            }

                            _interval = currWave.waveInterval;
                            SwitchState(SpawnerStates.SpawningWave);
                            break;
                        }

                        SwitchState(SpawnerStates.SpawningSegment);
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.Preparation:
                    if (_interval <= 0f)
                    {
                        SwitchState(SpawnerStates.SpawningWave);
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.FinalWait:
                    if (transform.childCount <= 0)
                    {
                        OnCompleted?.Invoke();
                    }
                    break;
                default:
                    break;
            }
        }

        private void ExitState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    break;
                case SpawnerStates.SpawningSegment:
                    break;
                case SpawnerStates.Waiting:
                    break;
                case SpawnerStates.Preparation:
                    break;
                case SpawnerStates.FinalWait:
                    break;
                default:
                    break;
            }
        }

        private void SpawningCompleted()
        {
            SwitchState(SpawnerStates.FinalWait);
        }

        private void SwitchState(SpawnerStates newState)
        {
            ExitState();
            currentState = newState;
            EnterState();
        }

        public void StopSpawner() => stopSpawning = true;
        public void ContinueSpawner() => stopSpawning = false;

        public void ResetSpawner()
        {
            waveIndex = 0;
            segmentIndex = 0;
            _interval = 0f;

            currWave = wavesSO.Waves[waveIndex];
            currSegment = currWave.waveSegments[segmentIndex];

            stopSpawning = false;

            SwitchState(SpawnerStates.SpawningWave);
        }

        private void SpawnEnemy()
        {
            GameObject unit = LevelManagement.PullObject(currSegment.enemyToSpawn, transform.position, Quaternion.identity, true);
            unit.transform.SetParent(transform);
            UnitBase unitScript = unit.GetComponent<UnitBase>();
            unitScript.OnUnitCompletedPath += unit =>
            {
                baseHealth -= unit.CurrentHealth;
                healthTMP.text = baseHealth.ToString();

                if (baseHealth <= 0f)
                {
                    OnBaseHealthDepleted?.Invoke();
                    ClearEntities();
                }
            };
        }

        public void ClearEntities()
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                transform.GetChild(0).GetComponent<UnitBase>().DestroyUnit();
            }
        }

        public void ModifyCash(CurrencyManager currencyManager)
        {
            this.currencyManager = currencyManager;
        }

        public void UpdateWaveUI()
        {
            waveCounterTMP.text = $"Wave: {WaveCount}/{wavesSO.Waves.Length}";
        }
    }

}