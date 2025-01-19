using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    sealed class IntermediateInput : InputBase {
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

            var velocity = moveAction.ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}