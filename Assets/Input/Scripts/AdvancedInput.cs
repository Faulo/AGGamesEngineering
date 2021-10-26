using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Scripts {
    public class AdvancedInput : MonoBehaviour {
        enum PlayerActions {
            Jump,
            Move,
        }

        [SerializeField]
        GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;
        [SerializeField]
        InputActionAsset referencedActionAsset = default;
        protected void OnEnable() {
            referencedActionAsset.Enable();
            bool isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;
        }
        protected void OnDisable() {
            referencedActionAsset.Disable();
            bool isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;
        }
        void Update() {
            bool isPressed = referencedActionAsset[nameof(PlayerActions.Jump)].phase == InputActionPhase.Started;
            var velocity = referencedActionAsset["Move"].ReadValue<Vector2>();

            referencedObject.transform.Translate(velocity * Time.deltaTime);
            referencedObject.GetComponent<Renderer>().sharedMaterial = isPressed
                ? lightMaterial
                : darkMaterial;
        }
    }
}