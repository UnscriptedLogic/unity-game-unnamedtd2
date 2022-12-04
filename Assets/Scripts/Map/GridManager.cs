using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridManagement
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] protected Vector2Int gridSize;
        [SerializeField] protected GameObject nodePrefab;
        [SerializeField] protected Grid grid;

        protected Dictionary<Tuple<int, int>, GridNode> gridNodes;

        public virtual void GenerateNodes()
        {
            gridNodes = new Dictionary<Tuple<int, int>, GridNode>();

            ForEachNode((x,y) =>
            {
                GameObject node = Instantiate(nodePrefab, new Vector3(grid.transform.position.x + x, 0, grid.transform.position.z + y), Quaternion.identity, transform);
                GridNode gridNode = node.GetComponent<GridNode>();
                gridNodes.Add(new Tuple<int, int>(x, y), gridNode);

                gridNode.coordx = x;
                gridNode.coordy = y;

                node.name = $"Node {x}, {y}";
            });
        }

        protected void ForEachNode(Action<int, int> method)
        {
            for (int x = -gridSize.x; x < gridSize.x; x++)
            {
                for (int y = -gridSize.y; y < gridSize.y; y++)
                {
                    method(x, y);
                }
            }
        }

        protected void ForEachNode(Action<GameObject> method)
        {
            for (int x = -gridSize.x; x < gridSize.x; x++)
            {
                for (int y = -gridSize.y; y < gridSize.y; y++)
                {
                    method(gridNodes[new Tuple<int, int>(x, y)].gameObject);
                }
            }
        }

        protected Vector3 GridCoordsToPosition(Vector3Int coords)
        {
            return grid.CellToWorld(coords) + new Vector3(grid.cellSize.x * 0.5f, transform.position.y, grid.cellSize.y * 0.5f);
        }

        protected Vector3 GetGridPosition(Vector3 position)
        {
            Vector3Int cellPosition = grid.WorldToCell(position);
            return grid.CellToWorld(cellPosition) + new Vector3(grid.cellSize.x * 0.5f, transform.position.y, grid.cellSize.y * 0.5f);
        }

        protected virtual void ResetMap()
        {

        }
    }
}