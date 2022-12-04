using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridManagement
{
    public class GridNode : MonoBehaviour
    {
        [HideInInspector] public int coordx, coordy;
        [HideInInspector] public float hCost;
        [HideInInspector] public float gCost;
        [HideInInspector] public GridNode cameFromNode;
        [HideInInspector] public bool isPath;

        public float Elevation => transform.position.y;
        public float fCost => hCost + gCost;
    }
}