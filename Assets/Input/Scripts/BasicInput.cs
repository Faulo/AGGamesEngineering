using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    sealed class BasicInput : InputBase {
        void Update() {
            isPressed = Keyboard.current.spaceKey.isPressed;

            var velocity = Gamepad.current.leftStick.ReadValue();
            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}