using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    sealed class AdvancedInput : InputBase {
        enum PlayerActions {
            Jump,
            Move,
        }

        [SerializeField]
        InputActionAsset referencedActionAsset = default;

        void OnEnable() {
            referencedActionAsset.Enable();
            bool isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;
        }

        void OnDisable() {
            referencedActionAsset.Disable();
            bool isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;
        }

        void Update() {
            isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;

            var velocity = referencedActionAsset["Move"].ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}