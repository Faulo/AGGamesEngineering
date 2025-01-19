using Slothsoft.UnityExtensions;
using TMPro;
using UnityEngine;

namespace AGGE.Input {
    public abstract class InputBase : MonoBehaviour {
        [SerializeField]
        internal GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        protected virtual void Start() {
            if (!referencedObject) {
                referencedObject = gameObject;
            }

            if (transform.TryGetComponentInChildren<TMP_Text>(out var text)) {
                text.text = gameObject.name;
            }
        }

        bool _isPressed;
        internal bool isPressed {
            get => _isPressed;
            set {
                _isPressed = value;

                referencedObject.transform.position = referencedObject.transform.position.WithZ(value ? 1 : 0);

                if (referencedObject.TryGetComponent<Renderer>(out var renderer)) {
                    renderer.sharedMaterial = value
                        ? lightMaterial
                        : darkMaterial;
                }
            }
        }

        internal Vector2 velocity;

        protected virtual void FixedUpdate() {
            referencedObject.transform.Translate(velocity.SwizzleXY() * Time.deltaTime);
        }
    }
}