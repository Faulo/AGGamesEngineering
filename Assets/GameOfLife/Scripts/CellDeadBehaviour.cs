using UnityEngine;

namespace AGGE.GameOfLife {
    public class CellDeadBehaviour : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.SetBool("isAlive", false);
        }
    }
}