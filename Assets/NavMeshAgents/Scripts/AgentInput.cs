using UnityEngine;
using UnityEngine.AI;


namespace AGGE.NavMeshAgents {
    public class AgentInput : MonoBehaviour {
        [SerializeField]
        NavMeshAgent agent = default;

        protected void Update() {
            if (Input.GetMouseButton(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100)) {
                    MoveTo(hit.point);
                }
            }
        }

        protected void MoveTo(Vector3 position) {
            agent.destination = position;
            agent.isStopped = false;
        }
    }
}