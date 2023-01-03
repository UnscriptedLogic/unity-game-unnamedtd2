using Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TowerManagement;
using UserInterfaceManagement;
using GameManagement;

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
        [SerializeField] private GameObject towerRange;
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

        [Header("Components")]
        [SerializeField] private CurrencyManager currencyManager;

        private List<Button> buildButtons;

        private void EnableTowerHoldComponents(bool enabled)
        {
            towerHold.GetComponentInChildren<TowerBase>().enabled = enabled;
            towerHold.GetComponentInChildren<Collider>().enabled = enabled;
        }

        public void InitBuildManager()
        {
            inputManager = InputManager.instance;
            inputManager.OnMouseMoving += InputManager_OnMouseMoving;
            inputManager.OnMouseDown += InputManager_OnMouseDown;
            inputManager.CancelOperation += InputManager_CancelOperation;

            for (int i = 0; i < availableTowers.TowerList.Count; i++)
            {
                GameObject towerButton = LevelManagement.PullObject(towerButtonPrefab, Vector3.zero, Quaternion.identity, true, towerListParent);
                towerButton.transform.localScale = Vector3.one;

                TowerSO towerSO;
                towerSO = availableTowers.TowerList[i];
                towerButton.GetComponent<TowerButton>().Initialize(availableTowers.TowerList[i], () =>
                {
                    isBuilding = true;
                    towerHoldSO = towerSO;
                    towerHold = Instantiate(towerSO.TowerLevels[0].towerPrefab);
                    EnableTowerHoldComponents(false);
                });
            }

            nodeHighlighter = Instantiate(nodeHighlighterPrefab);
            nodeHighlighter.SetActive(false);

            nodeHighlighterRenderer = nodeHighlighter.GetComponentInChildren<Renderer>();
            currentPlacementMaterial = nodeHighlighterRenderer.material;
        }

        private void InputManager_CancelOperation()
        {
            if (isBuilding)
            {
                towerRange.SetActive(false);
                isBuilding = false;
                Destroy(towerHold);

                nodeHighlighterRenderer.material = currentPlacementMaterial;
            }
        }

        private bool CanBePlaced(TowerSO towerSO, GameObject node)
        {
            if (Physics.CheckBox(node.transform.position + Vector3.up, Vector3.one * 0.4f))
            {
                return false;
            }

            List<PlacementCondition> placementConditions = new List<PlacementCondition>()
            {
                new PlacementCondition(IsNodeTypeValid(node, out string nodeType), $"Tower Not On {nodeType}")
            };

            for (int i = 0; i < placementConditions.Count; i++)
            {
                if (!placementConditions[i].ConditionMet)
                {
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

        private void InputManager_OnMouseDown(Vector2 mouseScreenPos, bool isOverUI)
        {
            if (isOverUI) return;

            if (isBuilding)
            {
                if (!canBePlaced)
                {
                    //Show warning
                    return;
                }

                EnableTowerHoldComponents(true);

                currencyManager.ModifyCurrency(ModificationType.Subtract, towerHoldSO.Cost);

                isBuilding = false;
                towerHold = null;
                nodeHighlighterRenderer.material = currentPlacementMaterial;
                towerRange.SetActive(false);
                return;
            }

            Ray ray = cam.ScreenPointToRay(mouseScreenPos);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 1000f, buildLayer))
            {
                Collider[] colliders = Physics.OverlapBox(hitinfo.collider.gameObject.transform.position + Vector3.up, Vector3.one * 0.4f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Tower"))
                    {
                        TowerBase tower = colliders[i].GetComponent<TowerBase>();
                        if (tower != null)
                        {
                            InspectWindow inspectWindow = UINavigator.Push("InspectWindow").GetComponent<InspectWindow>();
                            TowerUpgradeHandler upgradeHandler = tower.GetComponentInParent<TowerUpgradeHandler>();
                            LinkButtons(upgradeHandler, inspectWindow, colliders[i].gameObject);
                            return;
                        }
                    }
                }
            }

            if (towerRange.activeInHierarchy)
            {
                towerRange.SetActive(false);
            }

            if (UINavigator.GetTopPageName() == "InspectWindow")
            {
                UINavigator.Pop();
            }
        }

        private void LinkButtons(TowerUpgradeHandler upgradeHandler, InspectWindow inspectWindow, GameObject inspectedTower)
        {
            inspectWindow.ShowModal(upgradeHandler.TowerSO);

            int upgradeLevel;
            if (upgradeHandler.UpgradesChosen != null)
            {
                upgradeLevel = upgradeHandler.UpgradesChosen.Count;
            } else
            {
                upgradeLevel = 0;
            }

            inspectWindow.InitSellButton(upgradeHandler.TowerSO, upgradeLevel, () =>
            {
                LevelManagement.PushObject(inspectedTower);
                currencyManager.AddCash(upgradeHandler.TowerSO.TowerLevels[upgradeLevel].sellCost);
                
                if (UINavigator.GetTopPageName() == "InspectWindow")
                {
                    UINavigator.Pop();
                }
            });

            Button[] buttons = inspectWindow.InitUpgradeButtons(upgradeHandler.TowerSO, upgradeHandler.UpgradesChosen.ToArray(), inspectedTower);
            if (buttons != null)
            {
                for (int j = 0; j < buttons.Length; j++)
                {
                    int upgradeIndex = j;
                    buttons[j].onClick.AddListener(() =>
                    {
                        upgradeHandler.UpgradeTower(upgradeIndex);
                        LinkButtons(upgradeHandler, inspectWindow, inspectedTower);
                    });
                } 
            }

            TowerBase towerBase = inspectedTower.GetComponentInChildren<TowerBase>();
            towerRange.SetActive(true);
            towerRange.transform.localScale = Vector3.one * (towerBase.range * 2);
            towerRange.transform.position = towerBase.transform.position;
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

                    towerRange.SetActive(true);
                    
                    TowerBase towerBase = towerHold.GetComponentInChildren<TowerBase>();
                    towerRange.transform.localScale = Vector3.one * towerBase.range * 2;
                    towerRange.transform.position = towerHold.transform.position;
                }

                nodeHighlighter.transform.position = hitinfo.collider.transform.position + new Vector3(0f, verticalOffset, 0f);
            }
        }
    }
}