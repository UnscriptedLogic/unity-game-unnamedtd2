using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private float minY = 10f;
    [SerializeField] private float maxY = 80f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector2 bounds;
    [SerializeField] private Transform anchor;
    [SerializeField] private Transform panParent;

    private InputManager inputManager;
    private Vector2 axis;
    private Vector2 currentMousePos;
    private Quaternion rotationToLerp;
    private float rotationAngle;

    private void Start()
    {
        inputManager = InputManager.instance;
        inputManager.OnDirectionalMovement += InputManager_OnDirectionalMovement;
        inputManager.OnMouseMoving += InputManager_OnMouseMoving;
        inputManager.OnMouseScroll += InputManager_OnMouseScroll;
        inputManager.OnRotateCamera += InputManager_OnRotateCamera;
    }

    private void InputManager_OnRotateCamera(float obj)
    {
        rotationAngle += 90f * Mathf.Sign(obj);
        rotationToLerp = Quaternion.Euler(0, rotationAngle, 0);
    }

    private void InputManager_OnMouseScroll(float scroll)
    {
        Vector3 pos = transform.position + transform.forward * scroll * scrollSpeed * Time.deltaTime;

        if (pos.y >= maxY)
        {
            pos.x = transform.position.x;
            pos.y = maxY;
            pos.z = transform.position.z;
        } else if (pos.y <= minY)
        {
            pos.x = transform.position.x;
            pos.y = minY;
            pos.z = transform.position.z;
        }

        pos.y = Mathf.Clamp(pos.y, minY, maxY);
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
        if (axis.y > 0 || currentMousePos.y >= Screen.height - panBorderThickness)
        {
            anchor.Translate(anchor.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.y < 0 || currentMousePos.y <= panBorderThickness)
        {
            anchor.Translate(-anchor.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.x > 0 || currentMousePos.x >= Screen.width - panBorderThickness)
        {
            anchor.Translate(anchor.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (axis.x < 0 || currentMousePos.x <= panBorderThickness)
        {
            anchor.Translate(-anchor.right * panSpeed * Time.deltaTime, Space.World);
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -bounds.x, bounds.x);
        pos.z = Mathf.Clamp(pos.z, -bounds.y, bounds.y);
        transform.position = pos;

        anchor.rotation = Quaternion.Slerp(anchor.rotation, rotationToLerp, rotationSpeed * Time.deltaTime);
    }
}
