using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input.Tests {
    sealed class NullInput : MonoBehaviour {
        void OnEnable() {
            InputSystem.onAfterUpdate += HandleInput;
        }

        void OnDisable() {
            InputSystem.onAfterUpdate -= HandleInput;
        }

        void HandleInput() {
            transform.position += Vector3.up;
        }
    }
}