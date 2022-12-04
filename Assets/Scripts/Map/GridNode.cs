using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridManagement
{
    public class GridNode : MonoBehaviour
    {
        public int coordx, coordy;
        public float hCost;
        public float gCost;
        public GridNode cameFromNode;
        public bool isPath;

        public float Elevation => transform.position.y;
        public float fCost => hCost + gCost;
    }
}