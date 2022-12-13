using Core.Pathing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GridManagement
{
    public class MapManager : GridManager
    {
        [Header("Terrain Settings")]
        [Range(0, 1)][SerializeField] protected float terrainPerlinscale = 1f;
        [Range(0, 1)][SerializeField] protected float terrainTrimThreshold = 1f;
        [SerializeField] protected float terrainLower = 1f;
        [SerializeField] protected float elevationClamp;

        protected Vector2 cliffOrigin;

        [Header("Cliff Settings")]
        [SerializeField] protected bool generateCliffs;
        [Range(0, 1)][SerializeField] protected float cliffPerlinScale = 0.1f;
        [Range(0, 1)][SerializeField] protected float cliffThreshold = 0.5f;
        [SerializeField] protected float cliffLower = 1f;
        [SerializeField] protected GameObject fillPrefab;
        private List<GameObject> fill;

        protected Vector2 terrainOrigin;

        [Header("Path Settings")]
        [SerializeField] protected bool generatePath;
        [SerializeField] protected PathManager pathManager;
        [SerializeField] protected GameObject pathPrefab;

        [SerializeField] protected int seed;
        [SerializeField] protected bool random = true;

        [Header("Water Settings")]
        [SerializeField] protected bool alwaysGenerateWater;
        [SerializeField] protected Transform waterObject;
        [SerializeField] protected GameObject waterPrefab;

        [Header("End Points")]
        [SerializeField] protected GameObject homePrefab;
        [SerializeField] protected GameObject spawnPrefab;

        public override async Task GenerateNodes()
        {
            await base.GenerateNodes();

            if (random)
            {
                seed = UnityEngine.Random.Range(-9999, 9999);
            }

            UnityEngine.Random.InitState(seed);
            terrainOrigin = new Vector2(UnityEngine.Random.Range(-9999, 9999), UnityEngine.Random.Range(-9999, 9999));
            cliffOrigin = new Vector2(UnityEngine.Random.Range(-9999, 9999), UnityEngine.Random.Range(-9999, 9999));

            if (generatePath)
            {
                await pathManager.GeneratePath(gridNodes, gridSize, seed);
            }

            TerrainGeneration();
            CliffGeneration();
            LandFill();

            await Task.Yield();

            PathwayGen();
            GenerateWater();

            await Task.Yield();
        }

        protected void GenerateWater()
        {
            float elevation = 0f;
            if (alwaysGenerateWater)
            {
                elevation = pathManager.Path[0].Elevation + 0.3f;
            } 
            else
            {
                elevation = (int)UnityEngine.Random.Range(0, 3) + 0.3f;
                if (elevation > pathManager.Path[0].Elevation)
                {
                    elevation = pathManager.Path[0].Elevation + 0.3f;
                }

                if (elevation <= 0.3f)
                {
                    waterObject.gameObject.SetActive(false);
                    return;
                }
            }

            waterObject.position = new Vector3(waterObject.position.x, elevation, waterObject.position.z);
            waterObject.localScale = new Vector3(gridSize.x * 2 - 0.25f, elevation, gridSize.y * 2 - 0.25f);
            waterObject.gameObject.SetActive(true);

            ForEachNode(node =>
            {
                Vector3 position = new Vector3(node.transform.position.x, elevation, node.transform.position.z);
                if (!Physics.CheckSphere(position, 0.25f))
                {
                    GameObject waterNode = Instantiate(waterPrefab, position - Vector3.up * 0.5f, Quaternion.identity, transform);
                    Destroy(waterNode.transform.GetChild(0).gameObject);
                }
            });
        }

        private void PathwayGen()
        {
            float elevation = pathManager.Path[0].Elevation;
            GameObject homeNode = Instantiate(homePrefab, transform);
            homeNode.transform.position = pathManager.Path[0].transform.position + Vector3.up;

            for (int i = 0; i < pathManager.Path.Length; i++)
            {
                Vector3 pos = new Vector3(pathManager.Path[i].transform.position.x, elevation, pathManager.Path[i].transform.position.z);

                GameObject pathwayNode = Instantiate(pathPrefab, pos, Quaternion.identity, transform);

                GridNode existingNode = pathManager.Path[i];
                GridNode newNode = pathwayNode.GetComponent<GridNode>();

                newNode.coordx = existingNode.coordx;
                newNode.coordy = existingNode.coordy;
                newNode.hCost = existingNode.hCost;
                newNode.gCost = existingNode.gCost;
                newNode.cameFromNode = existingNode.cameFromNode;
                newNode.isPath = existingNode.isPath;
                pathManager.Path[i] = newNode;

                Collider[] colliders = Physics.OverlapBox(newNode.transform.position + Vector3.up * 1.5f, Vector3.one * 0.45f);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != newNode.gameObject)
                    {
                        Destroy(collider.gameObject);
                    }
                }
            }

            GameObject spawnNode = Instantiate(spawnPrefab, transform);
            Vector3 endNode = pathManager.Path[pathManager.Path.Length - 1].transform.position;
            spawnNode.transform.position = new Vector3(endNode.x, elevation + 1, endNode.z);

            pathManager.InitPath();
        }

        private void LandFill()
        {
            fill = new List<GameObject>();

            ForEachNode(node =>
            {
                int height = (int)node.transform.position.y;
                for (int i = 0; i < height; i++)
                {
                    GameObject fillObject = Instantiate(fillPrefab, new Vector3(node.transform.position.x, i, node.transform.position.z), Quaternion.identity, transform);
                    fill.Add(fillObject);
                }
            });
        }

        private void TerrainGeneration()
        {
            ForEachNode((x, y) =>
            {
                Transform node = gridNodes[new Tuple<int, int>(x, y)].transform;
                float perlinX = terrainOrigin.x + x * terrainPerlinscale;
                float perlinY = terrainOrigin.y + y * terrainPerlinscale;
                float elevation = Mathf.PerlinNoise(perlinX, perlinY);

                if (elevation >= terrainTrimThreshold)
                {
                    elevation = Mathf.RoundToInt(elevation * 10);
                    if (elevation > elevationClamp)
                    {
                        elevation = elevationClamp;
                    }

                    node.transform.position = new Vector3(node.position.x, elevation - terrainLower, node.position.z);
                }
            });
        }

        private void CliffGeneration()
        {
            if (!generateCliffs) return;
            
            ForEachNode((x, y) =>
            {
                Transform node = gridNodes[new Tuple<int, int>(x, y)].transform;
                float perlinX = cliffOrigin.x + x * cliffPerlinScale;
                float perlinY = cliffOrigin.y + y * cliffPerlinScale;
                float elevation = Mathf.PerlinNoise(perlinX, perlinY);

                if (elevation >= cliffThreshold)
                {
                    if (elevation > elevationClamp)
                    {
                        elevation = elevationClamp;
                    }

                    elevation = Mathf.RoundToInt(elevation * 10);
                    node.position = new Vector3(node.position.x, elevation - cliffLower, node.position.z);
                }
            });
        }

        protected override void ResetMap()
        {
            ClearFill();
            ResetTerrainNodes();
        }

        protected void ClearFill()
        {
            for (int i = 0; i < fill.Count; i++)
            {
                Destroy(fill[i]);
            }
            fill.Clear();
        }

        protected void ResetTerrainNodes()
        {
            List<Transform> nodeObjects = gridNodes.Select(n => n.Value.transform).ToList();
            for (int i = 0; i < nodeObjects.Count; i++)
            {
                nodeObjects[i].position = new Vector3(nodeObjects[i].position.x, 0f, nodeObjects[i].position.z);
            }
        }

        private void OnValidate()
        {
            if (gridNodes == null)
                return;

            if (random)
            {
                seed = UnityEngine.Random.Range(-9999, 9999);
            }

            ResetMap();

            UnityEngine.Random.InitState(seed);
            terrainOrigin = new Vector2(UnityEngine.Random.Range(-9999, 9999), UnityEngine.Random.Range(-9999, 9999));
            cliffOrigin = new Vector2(UnityEngine.Random.Range(-9999, 9999), UnityEngine.Random.Range(-9999, 9999));

            TerrainGeneration();
            CliffGeneration();
        }
    }
}