using Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TowerManagement;

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

        [Header("UI")]
        [SerializeField] private GameObject towerButtonPrefab;
        [SerializeField] private Transform towerListParent;
        private List<Button> buildButtons;

        private GameObject nodeHighlighter;

        private void Awake()
        {
            inputManager = InputManager.instance;
            inputManager.OnMouseMoving += InputManager_OnMouseMoving;

            
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

                nodeHighlighter.transform.position = hitinfo.collider.transform.position + new Vector3(0f, verticalOffset, 0f);
            }
        }
    }
}