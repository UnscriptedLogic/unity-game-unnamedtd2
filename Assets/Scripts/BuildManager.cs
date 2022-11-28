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
        [Header("Level Available Towers")]
        [SerializeField] private TowerListSO availableTowers;

        [Header("Build Settings")]
        private InputManager inputManager;
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask buildLayer;
        [SerializeField] private GameObject nodeHighlighterPrefab;
        [SerializeField] private float verticalOffset;

        private GameObject nodeHighlighter;
        
        [Header("UI")]
        [SerializeField] private GameObject towerButtonPrefab;
        [SerializeField] private Transform towerListParent;

        private List<Button> buildButtons;

        private bool isBuilding;
        private GameObject towerHold;

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
                    towerHold = Instantiate(availableTowers.TowerList[towerIndex].TowerLevels[0].towerPrefab);
                    towerHold.GetComponent<TowerBase>().enabled = false;
                });
            }
        }

        private void InputManager_OnMouseDown(Vector2 obj)
        {
            if (isBuilding)
            {
                isBuilding = false;
                towerHold.GetComponent<TowerBase>().enabled = true;
                towerHold = null;
            }
        }

        private void Start()
        {
            nodeHighlighter = Instantiate(nodeHighlighterPrefab);
            nodeHighlighter.SetActive(false);
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
                }

                nodeHighlighter.transform.position = hitinfo.collider.transform.position + new Vector3(0f, verticalOffset, 0f);
            }
        }
    }
}