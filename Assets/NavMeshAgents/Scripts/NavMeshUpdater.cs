using Unity.AI.Navigation;
using UnityEngine;

namespace AGGE.NavMeshAgents {
    public class NavMeshUpdater : MonoBehaviour {
        [SerializeField]
        NavMeshSurface surface;

        protected void Update() {
            surface.BuildNavMesh();
        }
    }
}