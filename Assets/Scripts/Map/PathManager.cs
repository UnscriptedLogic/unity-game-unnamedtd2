using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using GridManagement;
using System;
using System.Linq;
using UnityEngine.Rendering;
using Unity.VisualScripting;

namespace Core.Pathing
{
    public class PathManager : MonoBehaviour
    {
        private int seed = 10; //The seed for randomization
        [SerializeField] private int weightPointCount = 5; //The amount of 'crucial' points the path needs to meet (for proper path spread across the map)
        [SerializeField] private float weightPointDistance = 5f; //The amount of distance between the weightpoints;
        [SerializeField] private float pathCoverPercentage = 17.5f;
        [SerializeField] private bool allowOverlap = false;

        private Dictionary<Tuple<int, int>, GridNode> nodeDict;
        private Vector3[] waypoints;

        private GridNode[] weightPoints;
        private List<GridNode> nodes;
        private List<GridNode> path;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public GridNode[] Path { get => path.ToArray(); }
        public Vector3[] PathPoints { get => waypoints; }
        public CancellationTokenSource TokenSource => tokenSource;

        public async Task GeneratePath(Dictionary<Tuple<int, int>, GridNode> nodeDict, Vector2 gridSize, int seed)
        {
            this.seed = seed;
            this.nodeDict = nodeDict;

            bool invalid = true;
            int counter = 0;
            while (invalid)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    return;
                }

                UnityEngine.Random.InitState(this.seed);
                nodes = nodeDict.Select(n => n.Value).ToList();
                path = new List<GridNode>();

                await CreatePath(tokenSource, nodeDict, gridSize);

                invalid = !IsPathValid();
                if (invalid)
                {
                    counter++;
                    Reseed(nodes);
                }
            }

            Debug.Log("Path Completed");
        }

        private bool IsPathValid()
        {
            if ((float)path.Count / (float)nodes.Count * 100f < pathCoverPercentage)
            {
                Debug.Log($"Path too short: {((float)path.Count / (float)nodes.Count) * 100f}%");
                return false;
            }

            for (int i = 0; i < path.Count; i++)
            {
                int adjacentPaths = 0;
                GridNode[] neighbours = GetNeighboursOf(path[i]);
                for (int j = 0; j < neighbours.Length; j++)
                {
                    if (neighbours[j] != null)
                    {
                        if (neighbours[j].isPath)
                        {
                            adjacentPaths++;
                        }
                    }

                    if (adjacentPaths > 7)
                    {
                        Debug.Log($"Path too bunched up: {adjacentPaths}");
                        return false;
                    }
                }
            }

            return true;
        }

        private void Reseed(List<GridNode> nodes)
        {
            seed++;
            for (int j = 0; j < nodes.Count; j++)
            {
                nodes[j].isPath = false;
            }
        }

        private void GetWeightPoints()
        {
            int gridNodesLength = nodeDict.Count - 1;

            List<GridNode> newWeights = new List<GridNode>();
            newWeights.Add(nodes[UnityEngine.Random.Range(0, gridNodesLength)]);
            int retries = 0, maxRetries = 50;
            while (retries < maxRetries && newWeights.Count < weightPointCount)
            {
                int randomIndex = UnityEngine.Random.Range(0, gridNodesLength);
                bool isClose = false;
                for (int j = 0; j < newWeights.Count; j++)
                {
                    //if (PathFinder.GetDistance(nodes[randomIndex], newWeights[j]) < weightPointDistance)
                    if (Vector3.Distance(nodes[randomIndex].transform.position, newWeights[j].transform.position) < weightPointDistance)
                    {
                        isClose = true;
                        break;
                    }
                }

                retries++;
                if (!isClose)
                {
                    newWeights.Add(nodes[randomIndex]);
                    retries = 0;
                    
                }
            }

            weightPoints = newWeights.ToArray();
            Debug.Log($"Weight Points: {weightPoints.Length}");
        }

        private async Task CreatePath(CancellationTokenSource tokenSource, Dictionary<Tuple<int, int>, GridNode> nodeDict, Vector2 gridSize)
        {
            GetWeightPoints();

            await Task.Yield();

            for (int i = 0; i < weightPointCount - 1; i++)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    return;
                }

                if (i + 1 < weightPoints.Length)
                {
                    List<GridNode> subpath = PathFinder.GetPath(nodeDict, gridSize, weightPoints[i], weightPoints[i + 1], allowOverlap);

                    await Task.Yield();

                    if (subpath != null)
                    {
                        path.AddRange(subpath);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public GridNode[] GetNeighboursOf(GridNode node)
        {
            List<GridNode> neighbours = new List<GridNode>();

            AddIfNotNull(node.coordx - 1, node.coordy, ref neighbours);
            AddIfNotNull(node.coordx + 1, node.coordy, ref neighbours);
            AddIfNotNull(node.coordx, node.coordy + 1, ref neighbours);
            AddIfNotNull(node.coordx, node.coordy - 1, ref neighbours);

            AddIfNotNull(node.coordx - 1, node.coordy - 1, ref neighbours);
            AddIfNotNull(node.coordx + 1, node.coordy + 1, ref neighbours);
            AddIfNotNull(node.coordx - 1, node.coordy + 1, ref neighbours);
            AddIfNotNull(node.coordx + 1, node.coordy - 1, ref neighbours);

            return neighbours.ToArray();
        }

        private void AddIfNotNull(int x, int y, ref List<GridNode> gridNodes)
        {
            if (nodeDict.ContainsKey(new Tuple<int, int>(x, y)))
            {
                GridNode node = nodeDict[new Tuple<int, int>(x, y)];
                if (node != null)
                {
                    gridNodes.Add(node);
                }
            }
        }

        public void InitPath()
        {
            waypoints = new Vector3[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                waypoints[i] = path[i].transform.position;
            }
        }

        private void OnDisable()
        {
            tokenSource.Cancel();
        }
    }
}