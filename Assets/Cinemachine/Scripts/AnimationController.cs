using UnityEngine;

namespace AGGE.Cinemachine {
    public class AnimationController : MonoBehaviour {
        [SerializeField]
        Animator animator = default;
        [SerializeField]
        AvatarController controller = default;

        [SerializeField]
        string isWalkingStateName = default;
        [SerializeField]
        string isGroundedStateName = default;
        [SerializeField]
        string isJumpingStateName = default;

        void Update() {
            if (controller.isWalking) {
                animator.SetBool(isWalkingStateName, true);
            } else {
                animator.SetBool(isWalkingStateName, false);
            }
            if (controller.isJumping) {
                animator.SetBool(isJumpingStateName, true);
            } else {
                animator.SetBool(isJumpingStateName, false);
            }

            if (controller.isGrounded) {
                animator.SetBool(isGroundedStateName, true);
            } else {
                animator.SetBool(isGroundedStateName, false);
            }
        }
    }
}