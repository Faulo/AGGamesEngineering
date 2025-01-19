using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    sealed class ExpertInput : InputBase {

        ExpertInputAsset controls;

        void OnEnable() {
            controls = new ExpertInputAsset();
            controls.Enable();
        }

        void OnDisable() {
            controls.Disable();
        }

        void Update() {
            isPressed = controls.Player.Fly.phase == InputActionPhase.Started;
            var velocity = controls.Player.Move.ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}