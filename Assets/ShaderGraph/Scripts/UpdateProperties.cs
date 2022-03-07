using UnityEngine;

namespace AGGE.ShaderGraph {
    public class UpdateProperties : MonoBehaviour {
        [SerializeField]
        Renderer targetRenderer;
        [SerializeField]
        Material targetMaterial;

        protected void Awake() {
            SetUpComponents();

            targetMaterial = targetRenderer.sharedMaterial;
        }
        protected void OnValidate() {
            SetUpComponents();
        }
        void SetUpComponents() {
            if (!targetRenderer) {
                TryGetComponent(out targetRenderer);
            }
        }
    }
}
