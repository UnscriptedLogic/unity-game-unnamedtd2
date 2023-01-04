using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        public DefaultControls inputActions;

        private DefaultControls.DesktopControlsActions controls;

        public event Action<Vector2, Vector2> OnMouseMoving;
        public event Action<Vector2, bool> OnMouseDown;
        public event Action<Vector2, bool> OnMouseUp;
        public event Action<Vector2> OnDirectionalMovement;
        public event Action<float> OnMouseScroll;
        public event Action<float> OnRotateCamera;
        public event Action OnResetCamera;
        public event Action CancelOperation;

        private void Awake()
        {
            instance = this;
            inputActions = new DefaultControls();
            controls = inputActions.DesktopControls;
        }

        private void OnEnable()
        {
            inputActions.Enable();
            controls.CurrentMousePosDelta.started += CurrentMousePosDelta_performed;
            controls.Interact.started += Interact_started;
            controls.Interact.canceled += Interact_canceled;

            controls.DirectionalMovement.started += DirectionalMovement_started;
            controls.DirectionalMovement.canceled += DirectionalMovement_canceled;
            controls.Zoom.started += Zoom_started;
            controls.Zoom.canceled += Zoom_canceled;

            controls.CameraRotation.started += CameraRotation_started;
            controls.ResetCamera.started += ResetCamera_started;
            
            controls.CancelOperation.started += CancelOperation_started;
        }

        private void CancelOperation_started(InputAction.CallbackContext obj) => CancelOperation?.Invoke();

        private void ResetCamera_started(InputAction.CallbackContext obj) => OnResetCamera?.Invoke();
        private void CameraRotation_started(InputAction.CallbackContext obj) => OnRotateCamera?.Invoke(obj.ReadValue<float>());

        private void Zoom_canceled(InputAction.CallbackContext obj) => OnMouseScroll?.Invoke(obj.ReadValue<float>());
        private void Zoom_started(InputAction.CallbackContext obj) => OnMouseScroll?.Invoke(obj.ReadValue<float>());

        private void DirectionalMovement_started(InputAction.CallbackContext obj) => OnDirectionalMovement?.Invoke(obj.ReadValue<Vector2>());
        private void DirectionalMovement_canceled(InputAction.CallbackContext obj) => OnDirectionalMovement?.Invoke(obj.ReadValue<Vector2>());

        private void Interact_canceled(InputAction.CallbackContext obj) => OnMouseUp?.Invoke(controls.CurrentMousePosition.ReadValue<Vector2>(), IsTapOverUI(controls.CurrentMousePosition.ReadValue<Vector2>()));
        private void Interact_started(InputAction.CallbackContext obj) => OnMouseDown?.Invoke(controls.CurrentMousePosition.ReadValue<Vector2>(), IsTapOverUI(controls.CurrentMousePosition.ReadValue<Vector2>()));

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void CurrentMousePosDelta_performed(InputAction.CallbackContext obj)
        {
            Vector2 delta = obj.ReadValue<Vector2>();

            if (delta.magnitude > 0f)
            {
                OnMouseMoving?.Invoke(controls.CurrentMousePosition.ReadValue<Vector2>(), delta);
            }
        }

        //summary: Checks if the given screenpoint is over an UI element
        public bool IsTapOverUI(Vector2 tapPos)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = tapPos;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}