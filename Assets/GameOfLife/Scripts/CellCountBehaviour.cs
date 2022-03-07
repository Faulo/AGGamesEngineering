using UnityEngine;

namespace AGGE.GameOfLife {
    public class CellCountBehaviour : StateMachineBehaviour {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        CellController cell;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.TryGetComponent(out cell);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            int aliveNeighborCount = 0;
            for (int i = 0; i < cell.neighbors.Length; i++) {
                if (cell.neighbors[i].isAlive) {
                    aliveNeighborCount++;
                }
            }
            animator.SetInteger(nameof(aliveNeighborCount), aliveNeighborCount);
        }
    }
}