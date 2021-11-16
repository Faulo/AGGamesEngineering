using UnityEngine;

namespace AGGE.Cinemachine {
    public class AnimationController : MonoBehaviour {
        [SerializeField]
        Animator animator = default;
        [SerializeField]
        AvatarController controller = default;

        [SerializeField]
        string isWalkingStateName = string.Empty;
        [SerializeField]
        string isGroundedStateName = string.Empty;
        [SerializeField]
        string isJumpingStateName = string.Empty;

        void Update() {
            animator.SetBool(isWalkingStateName, controller.isWalking);
            animator.SetBool(isJumpingStateName, controller.isJumping);
            animator.SetBool(isGroundedStateName, controller.isGrounded);
        }
    }
}