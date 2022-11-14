using System;
using UnityEngine;
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
        public event Action<Vector2> OnMouseDown;
        public event Action<Vector2> OnMouseUp;

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
        }

        private void Interact_canceled(InputAction.CallbackContext obj)
        {
            OnMouseUp?.Invoke(controls.CurrentMousePosition.ReadValue<Vector2>());
        }

        private void Interact_started(InputAction.CallbackContext obj)
        {
            OnMouseDown?.Invoke(controls.CurrentMousePosition.ReadValue<Vector2>());
        }

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
    }
}