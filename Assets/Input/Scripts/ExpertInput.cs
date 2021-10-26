using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Scripts {
    public class ExpertInput : MonoBehaviour {
        [SerializeField]
        GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        ExpertInputAsset controls;
        protected void OnEnable() {
            controls = new ExpertInputAsset();
            controls.Enable();
        }
        protected void OnDisable() {
            controls.Disable();
        }
        void Update() {
            bool isPressed = controls.Player.Fly.phase == InputActionPhase.Started;
            var velocity = controls.Player.Move.ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
            referencedObject.GetComponent<Renderer>().sharedMaterial = isPressed
                ? lightMaterial
                : darkMaterial;
        }
    }
}