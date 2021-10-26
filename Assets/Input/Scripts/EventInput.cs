using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Scripts {
    public class EventInput : MonoBehaviour {
        [SerializeField]
        GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        ExpertInputAsset controls;
        Vector2 velocity;
        protected void OnEnable() {
            controls = new ExpertInputAsset();
            controls.Player.Fly.started += OnFly;
            controls.Player.Fly.performed += OnFly;
            controls.Player.Fly.canceled += OnFly;
            controls.Player.Move.started += OnMove;
            controls.Player.Move.performed += OnMove;
            controls.Player.Move.canceled += OnMove;
            controls.Enable();
        }
        protected void OnDisable() {
            controls.Disable();
        }

        void OnFly(InputAction.CallbackContext context) {
            bool isPressed = context.phase == InputActionPhase.Performed;

            referencedObject.GetComponent<Renderer>().sharedMaterial = isPressed
                ? lightMaterial
                : darkMaterial;
        }

        void OnMove(InputAction.CallbackContext context) {
            velocity = context.ReadValue<Vector2>();
        }
        protected void Update() {
            referencedObject.transform.Translate(velocity * Time.deltaTime);
        }
    }
}