using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AGGE.Input {
    public sealed class DirectDeviceInput : InputBase {
        void Update() {
            isPressed = Keyboard.current is { spaceKey: { isPressed: true } };

            if (Gamepad.current is { leftStick: StickControl stick }) {
                velocity = stick.ReadValue();
            }
        }
    }
}