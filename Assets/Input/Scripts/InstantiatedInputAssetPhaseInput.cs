using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    public sealed class InstantiatedInputAssetPhaseInput : InputBase {

        PlayerControls controls;

        void Awake() {
            controls = new();
        }

        void OnEnable() {
            controls.Enable();
        }

        void OnDisable() {
            controls.Disable();
        }

        void Update() {
            isPressed = controls.Player.Jump.phase is InputActionPhase.Started or InputActionPhase.Performed;
            velocity = controls.Player.Move.ReadValue<Vector2>();
        }
    }
}