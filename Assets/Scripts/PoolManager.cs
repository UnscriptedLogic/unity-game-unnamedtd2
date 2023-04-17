using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject poolItem;
        public Transform container;
        public int prespawnAmount;

        public Pool(GameObject poolItem, int prespawnAmount, Transform container)
        {
            this.poolItem = poolItem;
            this.container = container;
            this.prespawnAmount = prespawnAmount;
        }
    }

    public static PoolManager poolManagerInstance;

    public Transform poolContainer;
    public List<Pool> prePools;
    public Dictionary<string, Transform> pools { get; protected set; }

    protected virtual void Awake()
    {
        poolManagerInstance = this;

        prePools = new List<Pool>();
        pools = new Dictionary<string, Transform>();
        for (int i = 0; i < prePools.Count; i++)
        {
            for (int j = 0; j < prePools[i].prespawnAmount; j++)
            {
                GameObject poolItem = CreatePoolItem(prePools[i].poolItem);
                PushToPool(poolItem);
            }
        }
    }

    public virtual GameObject PullFromPool(GameObject poolRep, Vector3 position, Quaternion rotation, bool setActive = true)
    {
        string poolName = poolRep.name;
        GameObject poolItem;
        if (pools.ContainsKey(poolName))
        {
            //Greater than 1 because we never want a queue to be completely empty
            //because we wont know what type of gameobject belongs to that queue
            if (pools[poolName].childCount > 1)
            {
                poolItem = pools[poolName].GetChild(0).gameObject;
                poolItem.transform.SetParent(null);
            }
            else
            {
                poolItem = CreatePoolItem(poolRep);
            }

        }
        else
        {
            CreatePool(poolName, poolRep);
            poolItem = CreatePoolItem(poolRep);
        }

        poolItem.transform.SetPositionAndRotation(position, rotation);
        poolItem.SetActive(setActive);
        return poolItem;
    }

    public virtual GameObject PullFromPool(GameObject poolRep, Action<GameObject> method)
    {
        string poolName = poolRep.name;
        GameObject poolItem;
        if (pools.ContainsKey(poolName))
        {
            if (pools[poolName].childCount > 0)
            {
                poolItem = pools[poolName].GetChild(0).gameObject;
                poolItem.transform.SetParent(null);
            }
            else
            {
                poolItem = CreatePoolItem(poolRep);
            }
        }
        else
        {
            CreatePool(poolName, poolRep);
            poolItem = CreatePoolItem(poolRep);
        }

        method(poolItem);
        return poolItem;
    }

    public virtual void PushToPool(GameObject poolItem)
    {
        if (poolItem == null)
            return;

        string poolName = poolItem.name;
        if (!pools.ContainsKey(poolName))
        {
            CreatePool(poolName, poolItem);
        }

        for (int i = 0; i < prePools.Count; i++)
        {
            if (prePools[i].poolItem.name == poolItem.name)
            {
                poolItem.transform.SetParent(prePools[i].container);
            }
        }

        poolItem.SetActive(false);
        poolItem.transform.SetParent(pools[poolName]);
    }

    public virtual void PushToPoolAfter(float delay, GameObject poolItem) => StartCoroutine(DelayedPush(delay, poolItem));
    protected IEnumerator DelayedPush(float delay, GameObject poolItem)
    {
        if (poolItem == null || poolItem.activeInHierarchy == false)
            yield return null;

        yield return new WaitForSeconds(delay);
        PushToPool(poolItem);
    }

    protected virtual void CreatePool(string poolName, GameObject item)
    {
        Transform newPoolContainer = CreatePoolContainer(item.name);

        pools.Add(poolName, newPoolContainer);
        prePools.Add(new Pool(item, 0, newPoolContainer));
    }

    protected virtual GameObject CreatePoolItem(GameObject item)
    {
        GameObject newItem = Instantiate(item);
        newItem.name = item.name;
        newItem.SetActive(false);
        return newItem;
    }

    protected virtual Transform CreatePoolContainer(string itemName = "GameObject")
    {
        Transform container = new GameObject(itemName + "_Pool").transform;
        container.SetParent(poolContainer);
        return container;
    }
}