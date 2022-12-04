using System.Collections.Generic;
using UnityEngine;
using GridManagement;
using System;
using System.Linq;

namespace GridManagement
{
    public static class PathFinder
    {
        private static List<GridNode> openSet;
        private static List<GridNode> closedSet;

        public static List<GridNode> GetPath(Dictionary<Tuple<int, int>, GridNode> nodes, Vector2 gridSize, GridNode startNode, GridNode endNode, bool allowOverlap)
        {
            List<GridNode> nodesList = nodes.Select(n => n.Value).ToList();
            for (int i = 0; i < nodesList.Count; i++)
            {
                nodesList[i].gCost = float.MaxValue;
                nodesList[i].hCost = GetDistance(nodesList[i], endNode);
                nodesList[i].cameFromNode = null;
            }

            openSet = new List<GridNode>() { startNode };
            closedSet = new List<GridNode>();

            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, endNode);

            while (openSet.Count > 0)
            {
                GridNode currentNode = GetLowestFCost(openSet);

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (GridNode neighbourNode in GetNeighbours(nodes, gridSize, currentNode))
                {
                    if (closedSet.Contains(neighbourNode)) continue;

                    if (!allowOverlap)
                    {
                        if (neighbourNode.isPath)
                        {
                            closedSet.Add(neighbourNode);
                            continue;
                        }
                    }

                    float tmpGcost = currentNode.gCost + GetDistance(currentNode, neighbourNode);
                    if (tmpGcost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tmpGcost;
                        neighbourNode.hCost = GetDistance(neighbourNode, endNode);

                        if (!openSet.Contains(neighbourNode))
                        {
                            openSet.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private static List<GridNode> GetNeighbours(Dictionary<Tuple<int, int>, GridNode> nodes, Vector2 gridSize, GridNode node)
        {
            List<GridNode> neighbours = new List<GridNode>();

            if (node.coordx - 1 >= -gridSize.x)
            {
                neighbours.Add(nodes[new Tuple<int, int>(node.coordx - 1, node.coordy)]);
            }

            if (node.coordy - 1 >= -gridSize.y)
            {
                neighbours.Add(nodes[new Tuple<int, int>(node.coordx, node.coordy - 1)]);
            }

            if (node.coordx + 1 < gridSize.x)
            {
                neighbours.Add(nodes[new Tuple<int, int>(node.coordx + 1, node.coordy)]);
            }

            if (node.coordy + 1 < gridSize.y)
            {
                neighbours.Add(nodes[new Tuple<int, int>(node.coordx, node.coordy + 1)]);
            }

            return neighbours;
        }

        private static List<GridNode> CalculatePath(GridNode endNode)
        {
            List<GridNode> path = new List<GridNode>();
            path.Add(endNode);
            GridNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.cameFromNode;
                currentNode.isPath = true;
            }
            path.Reverse();
            return path;
        }

        public static float GetDistance(GridNode nodeA, GridNode nodeB)
        {
            return Vector3.Distance(nodeB.transform.position, nodeA.transform.position);
        }

        private static GridNode GetLowestFCost(List<GridNode> gridNodes)
        {
            GridNode lowestNode = gridNodes[0];
            for (int i = 1; i < gridNodes.Count; i++)
            {
                if (gridNodes[i].fCost < lowestNode.fCost)
                {
                    lowestNode = gridNodes[i];
                }
            }

            return lowestNode;
        }
    }
}