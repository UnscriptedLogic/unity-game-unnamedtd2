using UnityEngine;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using System.Collections.Generic;
using UnscriptedLogic.Experimental.Generation;
using System.Linq;
using System.Net.Mime;

public class MapManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GridSettings grid;

    private GridLogic<GameObject> gridGenerator;

    [Header("Path Settings")]
    [SerializeField] private float pointDist = 4;

    private List<Vector2> distributedPoints;
    private List<Vector3> assortedPoints;
    private Roy_T.AStar.Grids.Grid pathfinderGrid;

    private List<Cell> pathwayCells;
    public List<Vector3> Pathway { get; private set; }

    #region Singleton
    public static MapManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    } 
    #endregion

    private void Start()
    {
        GenerateMapGrid();
        GenerateMapPath();
    }

    private void GenerateMapGrid()
    {
        gridGenerator = new GridLogic<GameObject>(grid);
        gridGenerator.CreateGrid((cell, pos) =>
        {
            GameObject item = Instantiate(nodePrefab);
            item.name = $"{cell.GridCoords.x},{cell.GridCoords.y}";
            item.transform.position = new Vector3(pos.x, 0f, pos.y);
            item.transform.rotation = Quaternion.identity;
            item.transform.localScale = Vector3.one * grid.CellScale;
            item.transform.SetParent(transform);

            gridGenerator.gridCells.Add(cell, item);
        });
    }

    private void GenerateMapPath()
    {
        //Point distribution
        distributedPoints = PoissonDiscSampling.GeneratePoints(pointDist, grid.Size);

        if (distributedPoints.Count == 0)
        {
            GenerateMapPath();
            return;
        }

        //Point Arrangment
        List<Vector3> pointPositions = new List<Vector3>();
        for (int i = 0; i < distributedPoints.Count; i++)
        {
            pointPositions.Add(new Vector3(distributedPoints[i].x, 0f, distributedPoints[i].y));
        }
        assortedPoints = PointsConnectorShort.ConnectPoints(pointPositions);

        //Pathfinder grid setup
        GridSize gridSize = new GridSize(columns: grid.Size.x, rows: grid.Size.y);
        Size cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
        Velocity traversalVelocity = Velocity.FromKilometersPerHour(100);
        pathfinderGrid = Roy_T.AStar.Grids.Grid.CreateGridWithLateralConnections(gridSize, cellSize, traversalVelocity);
        PathFinder pathFinder = new PathFinder();

        pathwayCells = new List<Cell>();
        for (int i = 0; i < assortedPoints.Count; i++)
        {
            if (i - 1 >= 0)
            {
                GridPosition start = new GridPosition((int)assortedPoints[i - 1].x, (int)assortedPoints[i - 1].z);
                GridPosition end = new GridPosition((int)assortedPoints[i].x, (int)assortedPoints[i].z);
                Path path = pathFinder.FindPath(start, end, pathfinderGrid);

                //gridGenerator.GetCellFromGrid((int)path.Edges[0].Start.Position.X, (int)path.Edges[0].Start.Position.Y);
                for (int j = 0; j < path.Edges.Count; j++)
                {
                    Cell cell = gridGenerator.GetCellFromGrid((int)path.Edges[j].End.Position.X, (int)path.Edges[j].End.Position.Y);
                    pathwayCells.Add(cell);
                }
            }
        }

        CleanUpPath(pathwayCells);
    }

    private void CleanUpPath(List<Cell> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            //Cleans up back tracks
            if (i >= 2)
            {
                if (path[i].WorldCoords == path[i-2].WorldCoords)
                {
                    path.RemoveAt(i - 1);
                }
            }
        }

        int safetyCatch = 0;
        bool hasRoundabouts = true;
        while (hasRoundabouts || safetyCatch < 25)
        {
            hasRoundabouts = false;
            for (int i = 0; i < path.Count; i++)
            {
                if (i >= 4)
                {
                    //Potential 4 point roundabout
                    if (Vector3.Distance(path[i].WorldCoords, path[i - 3].WorldCoords) <= gridGenerator.Settings.CellSpacing + 0.1f)
                    {
                        //Debug.Log(Vector3.Distance(path[i].WorldCoords, path[i - 3].WorldCoords) <= gridGenerator.Settings.CellSpacing + 0.1f, gridGenerator.gridCells[path[i]]);

                        path.RemoveAt(i - 1);
                        path.RemoveAt(i - 2);

                        hasRoundabouts = true;
                        break;
                    }
                }
            }

            safetyCatch++;
        }

        //Create path visual after clean up
        Pathway = new List<Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            gridGenerator.gridCells[path[i]].SetActive(false);
            Pathway.Add(new Vector3(path[i].WorldCoords.x, 0f, path[i].WorldCoords.y));
        }
    }

    private void OnDrawGizmos()
    {
        if (assortedPoints == null)
            return;

        for (int i = 0; i < assortedPoints.Count; i++)
        {
            Gizmos.DrawSphere(assortedPoints[i] - new Vector3(grid.Size.x / 2f, -2f, grid.Size.x / 2f), 0.25f);

            if (i - 1 >= 0)
            {
                Gizmos.DrawLine(assortedPoints[i - 1] - new Vector3(grid.Size.x / 2f, -2f, grid.Size.x / 2f), assortedPoints[i] - new Vector3(grid.Size.x / 2f, -2f, grid.Size.x / 2f));
            }
        }
    }
}
