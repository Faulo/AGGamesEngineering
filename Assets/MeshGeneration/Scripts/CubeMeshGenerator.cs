using UnityEngine;

namespace MeshGeneration.Scripts {
    public class CubeMeshGenerator : MonoBehaviour {
        [SerializeField]
        MeshCollider attachedCollider = default;
        [SerializeField]
        MeshFilter attachedFilter = default;

        void OnValidate() {
            if (!attachedCollider) {
                TryGetComponent(out attachedCollider);
            }
            if (!attachedFilter) {
                TryGetComponent(out attachedFilter);
            }
        }
        void Awake() {
            var mesh = new Mesh();

            var vertices = new[] {
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
            };
            var uvs = new[] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };
            /*
             * A0
             * B1
             * C2
             * D3
             * E4
             * F5
             * G6
             * H7
            */
            int[] indices = new[] {
                0, 1, 5, 4,
                3, 0, 4, 7,
                2, 3, 7, 6,
                1, 2, 6, 5,
                5, 6, 7, 4,
                3, 2, 1, 0,
            };

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.SetIndices(indices, MeshTopology.Quads, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            attachedCollider.sharedMesh = mesh;
            attachedFilter.sharedMesh = mesh;
        }
    }
}