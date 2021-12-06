using TMPro;
using UnityEngine;

namespace AGGE.UI {
    public class GetName : MonoBehaviour {
        [SerializeField]
        Transform parent = default;

        [SerializeField]
        TextMeshProUGUI text = default;

        protected void OnValidate() {
            SetupText();
        }
        protected void Awake() {
            SetupText();
        }
        protected void Start() {
            SetupText();
        }

        void SetupText() {
            if (!parent) {
                parent = gameObject.transform.parent;
            }
            if (!text) {
                text = GetComponent<TextMeshProUGUI>();
            }

            text.text = parent.name;
        }
    }
}