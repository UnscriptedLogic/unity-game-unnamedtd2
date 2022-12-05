using Core;
using Core.Pathing;
using Game.Spawning;
using GridManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class LevelManagement : MonoBehaviour
    {
        public static LevelManagement baseInstance;

        [SerializeField] protected GridManager gridManager;
        [SerializeField] protected PathManager pathManager;
        [SerializeField] protected WaveSpawner waveSpawner;

        protected PoolManager poolManager;

        protected void Awake()
        {
            baseInstance = this;
        }

        protected async void Start()
        {
            poolManager = PoolManager.poolManagerInstance;
            await gridManager.GenerateNodes();
            waveSpawner.StartSpawner();
        }

        public static GameObject PullObject(GameObject prefab, Vector3 position, Quaternion rotation, bool setActive)
        {
            GameObject newObject = baseInstance.poolManager.PullFromPool(prefab, position, rotation, setActive);
            
            CheckForInterface<IRequiresPath>(newObject, obj =>
            {
                obj.InitWithPath(baseInstance.pathManager.PathPoints);
            });

            return newObject;
        }

        public static void PushObject(GameObject obj)
        {
            baseInstance.poolManager.PushToPool(obj);
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