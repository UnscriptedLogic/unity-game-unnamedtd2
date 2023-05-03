using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnscriptedLogic.Builders;
using UnscriptedLogic.MathUtils;
using UnscriptedLogic.Raycast;

public class OnBuildEventArgs : EventArgs
{
    public int buildIndex;
    public Vector3 position;
    public Quaternion rotation;
    public GameObject buildObject;
}

public class BuildManager : MonoBehaviour, IBuilder<TowerBase, GameObject>
{
    [SerializeField] private Material previewMaterial;
    [SerializeField] private Material invalidSpotMaterial;

    private RaycastHit hit;
    private BuildHandlerSimple<TowerBase, BuildManager, GameObject> buildHandler;
    private TowerDefenseManager tdManager;

    private bool isBuilding;
    private bool previewMode;
    private bool isValidSpot, setAsValid;
    private int towerBuildIndex;

    public BuildHandlerSimple<TowerBase, BuildManager, GameObject> BuildHandler => buildHandler;
    public GameObject[] buildableContainers => GetContainers();

    public event EventHandler<OnBuildEventArgs> OnBuild;

    private void Start()
    {
        tdManager = TowerDefenseManager.instance;

        buildHandler = new BuildHandlerSimple<TowerBase, BuildManager, GameObject>(this, buildableContainers.ToList());
        buildHandler.adminBuildConditions = new List<AdminBuildCondition<TowerBase>>()
        {
            new AdminBuildCondition<TowerBase>("On Node Layer", tower => LayerMask.NameToLayer("Node").Equals(hit.collider.gameObject.layer), "Invalid Build Position", "Valid Build Position"),
            new AdminBuildCondition<TowerBase>("Node Empty", tower => Physics.Raycast(hit.collider.transform.position, Vector3.up, 1f) == false),
            new AdminBuildCondition<TowerBase>("Cash", tower => tdManager.CurrentCash >= tdManager.AllTowerList.GetSOFromTower(tower).TowerCost)
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
            //Preview position handling
            if (buildHandler.PreviewObject == null)
            {
                buildHandler.Preview(towerBuildIndex, hit.collider.transform.position, Quaternion.identity);
                return;
            }

            buildHandler.PreviewObject.transform.position = hit.collider.transform.position;

            //Preview colour handling
            buildHandler.AssessPreviewAdminPass(out BuildResult adminResult);
            buildHandler.AssessPreviewLocalPass(hit.collider.transform.position, Quaternion.identity, out BuildResult localResult);

            isValidSpot = adminResult.Passed && localResult.Passed;
            if (setAsValid != isValidSpot)
            {
                SetBuildableMaterial(isValidSpot ? previewMaterial : invalidSpotMaterial);
                setAsValid = isValidSpot;
            }
        }
    }

    private void InputManager_OnMouseDown(Vector2 mousePos, bool overUI)
    {
        if (!isBuilding) return;

        if (overUI) return;

        if (RaycastLogic.FromMousePos3D(Camera.main, out hit))
        {
            buildHandler.Build(towerBuildIndex, hit.collider.transform.position, Quaternion.identity, OnConditionResult);

            buildHandler.ClearPreview();
            previewMode = false;
            isBuilding = false;

            setAsValid = false;
            isValidSpot = false;
        }
    } 
    #endregion

    public void AttemptBuild(int index)
    {
        isBuilding = true;
        previewMode = true;
        towerBuildIndex = index;
    }

    private void SetBuildableMaterial(Material material)
    {
        GameObject buildable = BuildHandler.PreviewObject;
        SkinnedMeshRenderer skinnedMeshRenderer = buildable.GetComponent<TowerBase>().TowerMeshRenderer;

        Material[] flashMats = new Material[skinnedMeshRenderer.materials.Length];
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            flashMats[i] = material;
        }

        skinnedMeshRenderer.materials = flashMats;
    }

    public GameObject[] GetContainers()
    {
        GameObject[] containers = new GameObject[tdManager.AllTowerList.TowerList.Count];
        for (int i = 0; i < tdManager.AllTowerList.TowerList.Count; i++)
        {
            containers[i] = tdManager.AllTowerList.TowerList[i].Prefab;
        }

        return containers;
    }

    public void OnConditionResult(BuildResult buildResult)
    {
        if (!buildResult.Passed)
        {
            Debug.Log(buildResult.Description);
        }
    }

    public void WhenCreateBuildable(int index, Vector3 position, Quaternion rotation, GameObject buildableContainer)
    {
        Instantiate(buildableContainer, position + (Vector3.up * 0.15f), rotation);
        buildableContainer.GetComponent<TowerBase>().enabled = true;

        OnBuild?.Invoke(this, new OnBuildEventArgs()
        {
            buildIndex = index,
            position = position,
            rotation = rotation,
            buildObject = buildableContainer
        });
    }

    public void WhenCreatePreview(int index, Vector3 position, Quaternion rotation, GameObject buildableContainer, out TowerBase tower, out GameObject towerObject)
    {
        towerObject = Instantiate(buildableContainer, position, rotation);
        towerObject.GetComponent<TowerBase>().enabled = false;
        Destroy(towerObject.GetComponent<BoxCollider>());

        tower = WhenGetBuildable(buildableContainer);
    }

    public TowerBase WhenGetBuildable(GameObject buildableObject) => buildableObject.GetComponent<TowerBase>();

    public void ClearPreview(TowerBase towerPreviewScript, GameObject towerPreviewObject)
    {
        Destroy(towerPreviewObject);
    }
}
