using Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TowerManagement;
using UserInterfaceManagement;

namespace BuildManagement
{
    public class BuildManager : MonoBehaviour
    {
        public class PlacementCondition
        {
            public bool ConditionMet { get; private set; }
            public string FailReason { get; private set; }

            public PlacementCondition(bool ConditionMet, string FailReason)
            {
                this.ConditionMet = ConditionMet;
                this.FailReason = FailReason;
            }
        }

        [Header("Level Available Towers")]
        [SerializeField] private TowerListSO availableTowers;

        [Header("Build Settings")]
        private InputManager inputManager;
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask buildLayer;
        [SerializeField] private float verticalOffset;

        private bool canBePlaced;
        private bool isBuilding;
        private GameObject towerHold;
        private TowerSO towerHoldSO;

        [Header("Node Highlighter")]
        [SerializeField] private GameObject nodeHighlighterPrefab;
        [SerializeField] private Material validPlacementMat;
        [SerializeField] private Material invalidPlacementMat;
        private Material currentPlacementMaterial;
        private GameObject nodeHighlighter;
        private Renderer nodeHighlighterRenderer;

        [Header("UI")]
        [SerializeField] private GameObject towerButtonPrefab;
        [SerializeField] private Transform towerListParent;

        private List<Button> buildButtons;

        private void Awake()
        {
            inputManager = InputManager.instance;
            inputManager.OnMouseMoving += InputManager_OnMouseMoving;
            inputManager.OnMouseDown += InputManager_OnMouseDown;

            int towerIndex;
            for (int i = 0; i < availableTowers.TowerList.Count; i++)
            {
                GameObject towerButton = Instantiate(towerButtonPrefab, towerListParent);
                towerIndex = i;
                towerButton.GetComponent<TowerButton>().Initialize(availableTowers.TowerList[towerIndex], () =>
                {
                    isBuilding = true;
                    towerHoldSO = availableTowers.TowerList[towerIndex];
                    towerHold = Instantiate(availableTowers.TowerList[towerIndex].TowerLevels[0].towerPrefab);
                    towerHold.GetComponent<TowerBase>().enabled = false;
                });
            }
        }

        private void Start()
        {
            nodeHighlighter = Instantiate(nodeHighlighterPrefab);
            nodeHighlighter.SetActive(false);

            nodeHighlighterRenderer = nodeHighlighter.GetComponentInChildren<Renderer>();
            currentPlacementMaterial = nodeHighlighterRenderer.material;
        }

        private bool CanBePlaced(TowerSO towerSO, GameObject node)
        {
            List<PlacementCondition> placementConditions = new List<PlacementCondition>()
            {
                new PlacementCondition(IsNodeTypeValid(node, out string nodeType), $"Tower Not On {nodeType}")
            };

            for (int i = 0; i < placementConditions.Count; i++)
            {
                if (!placementConditions[i].ConditionMet)
                {
                    Debug.Log(placementConditions[i].FailReason);
                    return false;
                }
            }

            return true;
        }

        private bool IsNodeTypeValid(GameObject node, out string toPlaceNodeType)
        {
            toPlaceNodeType = towerHoldSO.TowerPlacement.ToString();
            bool isTypeValid = node.CompareTag(toPlaceNodeType);
            return isTypeValid;
        }

        private void InputManager_OnMouseDown(Vector2 obj)
        {
            if (isBuilding)
            {
                if (!canBePlaced)
                {
                    //Show warning
                    return;
                }

                isBuilding = false;
                towerHold.GetComponent<TowerBase>().enabled = true;
                towerHold = null;
                nodeHighlighterRenderer.material = currentPlacementMaterial;
            }
        }

        private void InputManager_OnMouseMoving(Vector2 mouseScreenPos, Vector2 delta)
        {
            Ray ray = cam.ScreenPointToRay(mouseScreenPos);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 1000f, buildLayer))
            {
                if (!nodeHighlighter.activeInHierarchy)
                {
                    nodeHighlighter.SetActive(true);
                }

                if (isBuilding)
                {
                    towerHold.transform.position = hitinfo.collider.transform.position + Vector3.up * 0.5f;
                    canBePlaced = CanBePlaced(towerHoldSO, hitinfo.collider.gameObject);

                    nodeHighlighterRenderer.material = canBePlaced ? validPlacementMat : invalidPlacementMat; 
                }

                nodeHighlighter.transform.position = hitinfo.collider.transform.position + new Vector3(0f, verticalOffset, 0f);
            }
        }
    }
}