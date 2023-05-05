using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Builders;
using UnscriptedLogic.Raycast;

public class RangeVisualizer : MonoBehaviour
{
    [SerializeField] private Vector2 meshResoRange = new Vector2(0.1f, 0.6f);
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float verticalOffset = 0.25f;

    [SerializeField] private GameObject rangePrefab;
    private GameObject rangeVisualObj;
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    private float viewAngle = 365f;

    private float range;
    private Transform target;
    private TowerBase tower;
    private UnitBase unit;
    private bool drawRange;

    private void Start()
    {
        InputManager.instance.OnMouseDown += Instance_OnMouseDown;
        BuildManager.instance.OnPreviewing += Instance_OnPreviewing;
        BuildManager.instance.OnBuild += Instance_OnBuild;
    }

    private void Instance_OnBuild(object sender, OnBuildEventArgs e)
    {
        ClearValues();
        drawRange = false;
    }

    private void Instance_OnPreviewing(object sender, OnPreviewEventArgs e)
    {

    }

    private void LateUpdate()
    {
        if (!drawRange) return;

        if (tower)
        {
            target = tower.transform;
            range = tower.RangeHandler.Current;
        }

        if (unit)
        {
            target = unit.transform;
            range = 1f;
        }

        if (target == null)
        {
            tower = null;
            unit = null;
            drawRange = false;
            return;
        }

        DrawFOV(target, range);
    }

    private void Instance_OnMouseDown(Vector2 mousePos, bool isOverUI)
    {
        if (isOverUI) return;

        ClearValues();

        if (RaycastLogic.FromMousePos3D(Camera.main, out RaycastHit hit))
        {
            IInspectable inspectable = hit.transform.GetComponent<IInspectable>();
            if (inspectable != null)
            {
                if (inspectable as TowerBase)
                {
                    tower = inspectable as TowerBase;
                    target = tower.transform;
                    CreateMesh();
                }
                else if (inspectable as UnitBase)
                {
                    unit = inspectable as UnitBase;
                    target = unit.transform;
                    CreateMesh();
                }

                drawRange = true;
                return;
            }

            ClearMesh();
            drawRange = false;
            return;
        }
    }

    private void ClearValues()
    {
        tower = null;
        unit = null;
        range = 0f;
        target = null;
    }

    private void CreateMesh()
    {
        ClearMesh();

        rangeVisualObj = Instantiate(rangePrefab);
        rangeVisualObj.transform.SetParent(target);
        rangeVisualObj.transform.localPosition = new Vector3(0f, verticalOffset, 0f);
        rangeVisualObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        viewMeshFilter = rangeVisualObj.GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void ClearMesh()
    {
        Destroy(rangeVisualObj);
        viewMesh = null;
        viewMeshFilter = null;
    }

    private void DrawFOV(Transform target, float radius)
    {
        int stepCount = Mathf.RoundToInt(viewAngle * CalculateResolution(range));
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i < stepCount; i++)
        {
            float angle = target.eulerAngles.y - viewAngle / 2f + stepAngleSize * i;
            ViewCastInfo viewCast = ViewCast(angle);
            viewPoints.Add(viewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = target.InverseTransformPoint(viewPoints[i]);

            if ( i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(target.position, dir, out hit, range, obstacleMask))
        {
            return new ViewCastInfo()
            {
                hit = true,
                point = hit.point,
                dist = hit.distance,
                angle = globalAngle
            };
        } else
        {
            return new ViewCastInfo()
            {
                hit = false,
                point = target.position + dir * range,
                dist = range,
                angle = globalAngle
            };
        }
    }

    private Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle += target.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private float CalculateResolution(float range)
    {
        float maxResolution = meshResoRange.y - meshResoRange.x;
        float percentage = (range / maxRange) * 100f;

        return meshResoRange.y;
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float dist, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dist = dist;
            this.angle = angle;
        }
    }
}
