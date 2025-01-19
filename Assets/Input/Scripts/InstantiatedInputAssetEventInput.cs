using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    public sealed class InstantiatedInputAssetEventInput : InputBase {

        PlayerControls controls;

        void Awake() {
            controls = new();
        }

        void OnEnable() {
            controls.Player.Jump.started += OnFly;
            controls.Player.Jump.performed += OnFly;
            controls.Player.Jump.canceled += OnFly;
            controls.Player.Move.started += OnMove;
            controls.Player.Move.performed += OnMove;
            controls.Player.Move.canceled += OnMove;

            controls.Enable();
        }

        void OnDisable() {
            controls.Disable();

            controls.Player.Jump.started -= OnFly;
            controls.Player.Jump.performed -= OnFly;
            controls.Player.Jump.canceled -= OnFly;
            controls.Player.Move.started -= OnMove;
            controls.Player.Move.performed -= OnMove;
            controls.Player.Move.canceled -= OnMove;
        }

        void OnFly(InputAction.CallbackContext context) {
            isPressed = context.phase == InputActionPhase.Performed;
        }

        void OnMove(InputAction.CallbackContext context) {
            velocity = context.ReadValue<Vector2>();
        }
    }
}