using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    public sealed class SerializedInputActionInput : InputBase {
        [SerializeField]
        InputAction jumpAction = default;
        [SerializeField]
        InputAction moveAction = default;

        void OnEnable() {
            jumpAction.Enable();
            moveAction.Enable();
        }

        void OnDisable() {
            jumpAction.Disable();
            moveAction.Disable();
        }

        void Update() {
            isPressed = jumpAction.phase == InputActionPhase.Started;
            velocity = moveAction.ReadValue<Vector2>();
        }
    }
}