using UnityEngine;

namespace AGGE.GameOfLife {
    public class CellAliveBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool("isAlive", true);
        }
    }
}
