using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConstructComplete : MonoBehaviour
{
    [SerializeField] private GameObject nextTower;

    public void SpawnTower()
    {
        Instantiate(nextTower, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
