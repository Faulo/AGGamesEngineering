using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Scripts {
    public class IntermediateInput : MonoBehaviour {
        [SerializeField]
        GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;
        [SerializeField]
        InputAction jumpAction = default;
        [SerializeField]
        InputAction moveAction = default;
        protected void OnEnable() {
            jumpAction.Enable();
            moveAction.Enable();
        }
        protected void OnDisable() {
            jumpAction.Disable();
            moveAction.Disable();
        }
        void Update() {
            bool isPressed = jumpAction.phase == InputActionPhase.Started;
            var velocity = moveAction.ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
            referencedObject.GetComponent<Renderer>().sharedMaterial = isPressed
                ? lightMaterial
                : darkMaterial;
        }
    }
}