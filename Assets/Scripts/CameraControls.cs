using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 20f;

    [Tooltip("The thickness in pixels where the mouse is detected at the sides of the screen to start panning")]
    [SerializeField] private float panScreenDetectionThickness = 10f;

    [Tooltip("The bounds of of where the camera can go in world space")]
    [SerializeField] private Vector2 bounds;
    [SerializeField] private Transform anchor;
    [SerializeField] private Transform panParent;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private Vector2 scrollRange = new Vector2(10f, 80f);

    [Header("Rotate Settings")]
    [SerializeField] private float lerpSpeed = 10f;

    private InputManager inputManager;
    private Vector2 axis;
    private Vector2 currentMousePos;
    private Vector3 startCameraPos;
    private Quaternion rotationToLerp;
    private float rotationAngle;
    private bool isResetting;
    public bool stayDisabled;

    private void Start()
    {
        inputManager = InputManager.instance;
        EnableAllInput();
        startCameraPos = transform.localPosition;
    }

    public void InputManager_OnResetCamera()
    {
        rotationAngle = 0;
        rotationToLerp = Quaternion.Euler(0, 0, 0);
        isResetting = true;
        DisableAllInput();
    }

    private void InputManager_OnRotateCamera(float obj)
    {
        rotationAngle += 90f * Mathf.Sign(obj);
        rotationToLerp = Quaternion.Euler(0, rotationAngle, 0);
    }

    private void InputManager_OnMouseScroll(float scroll)
    {
        Vector3 pos = transform.position + transform.forward * scroll * scrollSpeed * Time.deltaTime;

        if (pos.y >= scrollRange.y)
        {
            pos.x = transform.position.x;
            pos.y = scrollRange.y;
            pos.z = transform.position.z;
        }
        else if (pos.y <= scrollRange.x)
        {
            pos.x = transform.position.x;
            pos.y = scrollRange.x;
            pos.z = transform.position.z;
        }

        pos.y = Mathf.Clamp(pos.y, scrollRange.x, scrollRange.y);
        transform.position = pos;
    }

    private void InputManager_OnMouseMoving(Vector2 currentPos, Vector2 delta)
    {
        currentMousePos = currentPos;
    }

    private void InputManager_OnDirectionalMovement(Vector2 obj)
    {
        axis = obj;
    }

    private void Update()
    {
        anchor.rotation = Quaternion.Slerp(anchor.rotation, rotationToLerp, lerpSpeed * Time.deltaTime);

        if (isResetting)
        {
            anchor.position = Vector3.Lerp(anchor.position, new Vector3(0f, anchor.position.y, 0f), lerpSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, startCameraPos, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(anchor.position, new Vector3(0f, anchor.position.y, 0f)) <= 0.1f)
            {
                isResetting = false;

                if (!stayDisabled)
                {
                    EnableAllInput();
                    stayDisabled = false;
                } 
            }
            return;
        }

        if (axis.y > 0 || currentMousePos.y >= Screen.height - panScreenDetectionThickness)
        {
            anchor.Translate(anchor.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.y < 0 || currentMousePos.y <= panScreenDetectionThickness)
        {
            anchor.Translate(-anchor.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.x > 0 || currentMousePos.x >= Screen.width - panScreenDetectionThickness)
        {
            anchor.Translate(anchor.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.x < 0 || currentMousePos.x <= panScreenDetectionThickness)
        {
            anchor.Translate(-anchor.right * panSpeed * Time.deltaTime, Space.World);
        }

        Vector3 pos = anchor.position;
        pos.x = Mathf.Clamp(pos.x, -bounds.x, bounds.x);
        pos.z = Mathf.Clamp(pos.z, -bounds.y, bounds.y);
        anchor.position = pos;
    }

    public void DisableAllInput()
    {
        inputManager.OnDirectionalMovement -= InputManager_OnDirectionalMovement;
        inputManager.OnMouseMoving -= InputManager_OnMouseMoving;
        inputManager.OnMouseScroll -= InputManager_OnMouseScroll;
        inputManager.OnRotateCamera -= InputManager_OnRotateCamera;
        inputManager.OnResetCamera -= InputManager_OnResetCamera;
    }

    public void EnableAllInput()
    {
        inputManager.OnDirectionalMovement += InputManager_OnDirectionalMovement;
        inputManager.OnMouseMoving += InputManager_OnMouseMoving;
        inputManager.OnMouseScroll += InputManager_OnMouseScroll;
        inputManager.OnRotateCamera += InputManager_OnRotateCamera;
        inputManager.OnResetCamera += InputManager_OnResetCamera;
    }
}