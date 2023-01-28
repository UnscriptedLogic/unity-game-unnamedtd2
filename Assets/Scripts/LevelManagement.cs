using BuildManagement;
using Core;
using Core.Pathing;
using Game.Spawning;
using GridManagement;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using TowerManagement;
using UnitManagement;
using UnityEditor.Rendering;
using UnityEngine;

namespace GameManagement
{
    public class LevelManagement : MonoBehaviour
    {
        public static LevelManagement baseInstance;

        [SerializeField] protected CameraControls cameraControls;
        [SerializeField] protected BuildManager buildManager;
        [SerializeField] protected GridManager gridManager;
        [SerializeField] protected PathManager pathManager;
        [SerializeField] protected WaveSpawner waveSpawner;
        [SerializeField] protected CurrencyManager currencyManager;
        [SerializeField] protected PoolManager poolManager;
        [SerializeField] protected UnitEventHandler unitEventHandler;
        [SerializeField] protected SoundManager soundManager; 

        protected void Awake()
        {
            baseInstance = this;
        }

        protected async void Start()
        {
            await gridManager.GenerateNodes();
            buildManager.InitBuildManager();
            currencyManager.InitCash();
            waveSpawner.StartSpawner();

            waveSpawner.OnBaseHealthDepleted += OnBaseHealthDepleted;
            waveSpawner.OnCompleted += OnCompleted;
        }

        private void OnCompleted()
        {
            UINavigator.PopAll();
            UINavigator.Push("GameWon");

            cameraControls.InputManager_OnResetCamera();
            cameraControls.DisableAllInput();
        }

        private void OnBaseHealthDepleted()
        {
            waveSpawner.ClearEntities();
            waveSpawner.StopSpawner();
            UINavigator.PopAll();
            UINavigator.Push("GameOver");

            cameraControls.InputManager_OnResetCamera();
            cameraControls.DisableAllInput();
        }

        public static GameObject PullObject(GameObject prefab, Vector3 position, Quaternion rotation, bool setActive, Transform parent = null)
        {
            GameObject newObject = baseInstance.poolManager.PullFromPool(prefab, position, rotation, setActive);

            CheckForInterface<IUsesUnitEvent>(newObject, obj =>
            {
                obj.InitWithUnitEventHandler(baseInstance.unitEventHandler);
            });

            CheckForInterface<IRequiresPath>(newObject, obj =>
            {
                obj.InitWithPath(baseInstance.pathManager.PathPoints);
            });

            CheckForInterface<IListensToCurrency>(newObject, obj =>
            {
                baseInstance.currencyManager.OnCashModified += obj.OnCurrencyChanged;
            });

            CheckForInterface<IModifiesCurrency>(newObject, obj =>
            {
                obj.ModifyCash(baseInstance.currencyManager);
            });

            newObject.transform.SetParent(parent);
            return newObject;
        }

        public static void PushObject(GameObject newObject)
        {
            CheckForInterface<IListensToCurrency>(newObject, obj =>
            {
                baseInstance.currencyManager.OnCashModified -= obj.OnCurrencyChanged;
            });
            
            baseInstance.poolManager.PushToPool(newObject);
        }

        public static IEnumerator PushObjectAfter(GameObject newObject, float delay)
        {
            yield return new WaitForSeconds(delay);

            CheckForInterface<IListensToCurrency>(newObject, obj =>
            {
                baseInstance.currencyManager.OnCashModified -= obj.OnCurrencyChanged;
            });

            baseInstance.poolManager.PushToPool(newObject);
        }

        public static void CheckForInterface<T>(GameObject objToCheck, Action<T> method)
        {
            T[] interfaces = objToCheck.GetComponents<T>();
            for (int i = 0; i < interfaces.Length; i++)
            {
                method(interfaces[i]);
            }
        }
    }
}