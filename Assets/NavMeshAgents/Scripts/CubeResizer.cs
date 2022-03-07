using UnityEngine;

namespace AGGE.NavMeshAgents {
    public class CubeResizer : MonoBehaviour {
        [SerializeField]
        Vector3 minSize;
        [SerializeField]
        Vector3 maxSize;

        protected void Update() {
            transform.localScale = Vector3.Lerp(minSize, maxSize, Mathf.Abs(Mathf.Sin(Time.time)));
        }
    }
}