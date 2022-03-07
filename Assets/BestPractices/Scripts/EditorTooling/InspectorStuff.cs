using UnityEngine;

namespace AGGE.BestPractices.EditorTooling {
    public class InspectorStuff : MonoBehaviour {

        #region SerializeField and TryGetComponent

        // announce which components your script changes via SerializeField
        [SerializeField]
        Renderer targetRenderer;

        void SetUpComponents() {
            // TryGetComponent can automatically infer the type
            // targetRenderer = GetComponent<Renderer>();

            if (!targetRenderer) {
                TryGetComponent(out targetRenderer);
            }
        }


        protected void Awake() {
            SetUpComponents();
        }
        protected void OnValidate() {
            SetUpComponents();
        }

        #endregion

        #region public getter, private setter

        // fields should almost never be public
        // [SerializeField]
        // public Collider targetCollider;

        // have a public accesor for a serialized field
        [SerializeField]
        Collider m_targetCollider;
        public Collider targetColliderA => m_targetCollider;

        // use auto-generated fields
        [field: SerializeField]
        public Collider targetColliderB { get; private set; }

        #endregion
    }
}
