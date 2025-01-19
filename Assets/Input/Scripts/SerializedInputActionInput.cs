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

        protected override void ProcessInput() {
            isPressed = jumpAction.phase is InputActionPhase.Started or InputActionPhase.Performed;
            velocity = moveAction.ReadValue<Vector2>();
        }
    }
}