using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.Input {
    public class BasicInput : MonoBehaviour {
        [SerializeField]
        GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        void Update() {
            bool isPressed = Keyboard.current.spaceKey.isPressed;
            var velocity = Gamepad.current.leftStick.ReadValue();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
            referencedObject.GetComponent<Renderer>().sharedMaterial = isPressed
                ? lightMaterial
                : darkMaterial;
        }
    }
}