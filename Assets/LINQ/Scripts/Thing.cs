using UnityEngine;

namespace AGGE.LINQ {
    public class Thing : MonoBehaviour {
        [SerializeField]
        public Color color = Color.white;
        [SerializeField]
        public Vector3Int position = Vector3Int.zero;
        [SerializeField]
        public Vector3Int size = Vector3Int.one;
        [SerializeField]
        public PrimitiveType form = PrimitiveType.Cube;
        [SerializeField]
        public bool isSelected = false;

        MeshRenderer attachedRenderer;
        MeshFilter attachedFilter;
        Material material;

        protected void Awake() {
            TryGetComponent(out attachedRenderer);
            TryGetComponent(out attachedFilter);
            material = attachedRenderer.material;
        }
        protected void Start() {
            UpdateState();
        }
        protected void Update() {
            UpdateState();
        }
        void UpdateState() {
            material.color = color;
            if (isSelected) {
                material.EnableKeyword("_EMISSION");
            } else {
                material.DisableKeyword("_EMISSION");
            }
            transform.localPosition = position;
            transform.localScale = size;
            attachedFilter.sharedMesh = Resources.GetBuiltinResource<Mesh>($"{form}.fbx");
        }
    }
}
