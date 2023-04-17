using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnscriptedLogic.Builders;
using UnscriptedLogic.Raycast;

public class BuildManager : MonoBehaviour, IBuilder<Tower, GameObject>
{
    [Header("Build Settings")]
    [SerializeField] private GameObject[] towers;

    [SerializeField] private bool previewMode;
    private RaycastHit hit;
    private BuildHandlerSimple<Tower, BuildManager, GameObject> buildHandler;

    public GameObject[] buildableContainers => towers;
    public BuildHandlerSimple<Tower, BuildManager, GameObject> BuildHandler => buildHandler;

    private void Start()
    {
        buildHandler = new BuildHandlerSimple<Tower, BuildManager, GameObject>(this, buildableContainers.ToList());
        buildHandler.adminBuildConditions = new List<AdminBuildCondition<Tower>>()
        {
            new AdminBuildCondition<Tower>("On Node Layer", tower => LayerMask.NameToLayer("Node").Equals(hit.collider.gameObject.layer), "Invalid Build Position", "Valid Build Position"),
            new AdminBuildCondition<Tower>("Node Empty", tower => Physics.Raycast(hit.collider.transform.position, Vector3.up, 1f) == false),
        };

        InputManager.instance.OnMouseDown += InputManager_OnMouseDown;
        InputManager.instance.OnMouseMoving += InputManager_OnMouseMoving;
    }

    #region Mouse Detection
    private void InputManager_OnMouseMoving(Vector2 mousePos, Vector2 delta)
    {
        if (!previewMode) return;

        if (RaycastLogic.FromMousePos3D(Camera.main, out hit))
        {
            if (buildHandler.PreviewObject == null)
            {
                buildHandler.Preview(0, hit.collider.transform.position, Quaternion.identity);
                return;
            }

            buildHandler.PreviewObject.transform.position = hit.collider.transform.position;
        }
    }

    private void InputManager_OnMouseDown(Vector2 mousePos, bool overUI)
    {
        if (overUI) return;

        if (RaycastLogic.FromMousePos3D(Camera.main, out hit))
        {
            Debug.Log("Build");
            buildHandler.Build(0, hit.collider.transform.position, Quaternion.identity, OnConditionResult);

            buildHandler.ClearPreview();
            previewMode = false;
        }
    } 
    #endregion

    public void OnConditionResult(BuildResult buildResult)
    {
        if (!buildResult.Passed)
        {
            Debug.Log(buildResult.Description);
        }
    }

    public void WhenCreateBuildable(int index, Vector3 position, Quaternion rotation, GameObject buildableContainer) => Instantiate(buildableContainer, position + (Vector3.up * 0.15f), rotation);

    public void WhenCreatePreview(int index, Vector3 position, Quaternion rotation, GameObject buildableContainer, out Tower tower, out GameObject towerObject)
    {
        towerObject = Instantiate(buildableContainer, position, rotation);
        Destroy(towerObject.GetComponent<BoxCollider>());

        tower = WhenGetBuildable(buildableContainer);
        tower.enabled = false;
    }

    public Tower WhenGetBuildable(GameObject buildableObject) => buildableObject.GetComponent<Tower>();

    public void ClearPreview(Tower towerPreviewScript, GameObject towerPreviewObject)
    {
        Destroy(towerPreviewObject);
    }
}
