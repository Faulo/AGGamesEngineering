using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    public sealed class ReferencedInputActionAssetInput : InputBase {
        enum PlayerActions {
            Jump,
            Move,
        }

        [SerializeField]
        InputActionAsset referencedActionAsset = default;

        void OnEnable() {
            referencedActionAsset.Enable();
        }

        void OnDisable() {
            referencedActionAsset.Disable();
        }

        protected override void ProcessInput() {
            isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase is InputActionPhase.Started or InputActionPhase.Performed;
            velocity = referencedActionAsset["Move"].ReadValue<Vector2>();
        }
    }
}