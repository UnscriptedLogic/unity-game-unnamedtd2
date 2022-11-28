using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridManagement
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private Grid grid;

        private void Start()
        {
            GenerateNodes();
        }

        private void GenerateNodes()
        {
            for (int x = -gridSize.x; x < gridSize.x; x++)
            {
                for (int y = -gridSize.y; y < gridSize.y; y++)
                {
                    GameObject node = Instantiate(nodePrefab, new Vector3(grid.transform.position.x + x, 0, grid.transform.position.z + y), Quaternion.identity);
                }
            }
        }

        private Vector3 GridCoordsToPosition(Vector3Int coords)
        {
            return grid.CellToWorld(coords) + new Vector3(grid.cellSize.x * 0.5f, transform.position.y, grid.cellSize.y * 0.5f);
        }

        private Vector3 GetGridPosition(Vector3 position)
        {
            Vector3Int cellPosition = grid.WorldToCell(position);
            return grid.CellToWorld(cellPosition) + new Vector3(grid.cellSize.x * 0.5f, transform.position.y, grid.cellSize.y * 0.5f);
        }
    }
}