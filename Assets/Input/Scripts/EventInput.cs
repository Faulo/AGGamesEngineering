using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    sealed class EventInput : InputBase {
        ExpertInputAsset controls;

        Vector2 velocity;

        void OnEnable() {
            controls = new ExpertInputAsset();
            controls.Player.Fly.started += OnFly;
            controls.Player.Fly.performed += OnFly;
            controls.Player.Fly.canceled += OnFly;
            controls.Player.Move.started += OnMove;
            controls.Player.Move.performed += OnMove;
            controls.Player.Move.canceled += OnMove;
            controls.Enable();
        }
        void OnDisable() {
            controls.Disable();
        }

        void OnFly(InputAction.CallbackContext context) {
            isPressed = context.phase == InputActionPhase.Performed;
        }

        void OnMove(InputAction.CallbackContext context) {
            velocity = context.ReadValue<Vector2>();
        }

        void Update() {
            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}