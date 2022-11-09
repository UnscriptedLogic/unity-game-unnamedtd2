using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
    public class TowerBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] private float damage = 1f;
        [SerializeField] private float range = 1f;
        [SerializeField] private float reloadTime = 1f;

        [Header("Base Components")]
        [SerializeField] private Transform[] rotationHeads;
        [SerializeField] private Transform[] shootAnchors;
    }
}