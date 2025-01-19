using Slothsoft.UnityExtensions;
using TMPro;
using UnityEngine;

namespace AGGE.Input {
    abstract class InputBase : MonoBehaviour {
        [SerializeField]
        internal GameObject referencedObject = default;
        [SerializeField]
        Material darkMaterial = default;
        [SerializeField]
        Material lightMaterial = default;

        protected virtual void Start() {
            if (transform.TryGetComponentInChildren<TMP_Text>(out var text)) {
                text.text = gameObject.name;
            }
        }

        internal bool isPressed {
            set {
                referencedObject.GetComponent<Renderer>().sharedMaterial = value
                    ? lightMaterial
                    : darkMaterial;
            }
        }
    }
}