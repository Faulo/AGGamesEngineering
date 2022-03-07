using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.CharacterController {
    public class AvatarCharacterController : MonoBehaviour {
        [SerializeField]
        UnityEngine.CharacterController attachedCharacter;
        [SerializeField]
        ControlScheme scheme = ControlScheme.Velocity;
        [SerializeField, Range(0, 100)]
        float jumpSpeed = 5;
        [SerializeField, Range(0, 100)]
        float movementSpeed = 5;
        [SerializeField, Range(0, 10)]
        float accelerationDuration = 1;

        [Header("Drag")]
        [SerializeField]
        Vector3 dragVelocity = Vector3.zero;
        [SerializeField, Range(0, 10)]
        float dragDuration = 1;
        Vector3 dragAcceleration;

        void OnValidate() {
            if (!attachedCharacter) {
                TryGetComponent(out attachedCharacter);
            }
        }

        Vector2 movement;
        bool isJumping;
        [SerializeField]
        Vector3 velocity;
        Vector3 acceleration;

        void Update() {
            movement = Gamepad.current.leftStick.ReadValue();
            isJumping = Gamepad.current.aButton.isPressed;
        }

        void FixedUpdate() {
            dragVelocity = Vector3.zero;
            var colliders = Physics.OverlapSphere(transform.position, attachedCharacter.radius, -1, QueryTriggerInteraction.Collide);
            if (colliders.Length > 0) {
                foreach (var collider in colliders) {
                    if (collider.TryGetComponent<DragSource>(out var drag)) {
                        dragVelocity = drag.velocity;
                        break;
                    }
                }
            }

            switch (scheme) {
                case ControlScheme.Position:
                    break;
                case ControlScheme.Velocity:
                    var targetVelocity = new Vector3(
                        movement.x * movementSpeed,
                        velocity.y,
                        movement.y * movementSpeed
                    );
                    velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref acceleration, accelerationDuration);
                    velocity = Vector3.SmoothDamp(velocity, dragVelocity, ref dragAcceleration, dragDuration);
                    if (isJumping && attachedCharacter.isGrounded) {
                        velocity.y = jumpSpeed;
                    }
                    velocity += Physics.gravity * Time.deltaTime;

                    attachedCharacter.Move(velocity * Time.deltaTime);
                    break;
                case ControlScheme.Acceleration:
                    //attachedRigidbody.AddForce(Physics.gravity, ForceMode.);
                    break;
                default:
                    break;
            }
        }
    }
}