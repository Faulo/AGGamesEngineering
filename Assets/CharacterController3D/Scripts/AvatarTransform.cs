using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.CharacterController3D {
    public class AvatarTransform : MonoBehaviour {
        [SerializeField, Range(0, 100)]
        float jumpSpeed = 5;

        [SerializeField]
        ControlScheme scheme = ControlScheme.Velocity;

        [SerializeField, Range(0, 100)]
        float movementSpeed = 5;
        [SerializeField, Range(0, 10)]
        float accelerationDuration = 1;
        [SerializeField, Range(0, 100)]
        float maxPosition = 10;

        [Header("Drag")]
        [SerializeField]
        Vector3 dragVelocity = Vector3.zero;
        [SerializeField, Range(0, 10)]
        float dragDuration = 1;
        Vector3 dragAcceleration;

        Vector2 movement;
        [SerializeField]
        Vector3 velocity;
        Vector3 acceleration;
        bool isJumping;

        void Update() {
            movement = Gamepad.current.leftStick.ReadValue();
            isJumping = Gamepad.current.aButton.isPressed;
        }

        void FixedUpdate() {
            dragVelocity = Vector3.zero;
            var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, -1, QueryTriggerInteraction.Collide);
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
                    transform.position = new Vector3(
                        movement.x * maxPosition,
                        transform.position.y,
                        movement.y * maxPosition
                    );
                    break;
                case ControlScheme.Velocity:
                    var targetVelocity = new Vector3(
                        movement.x * movementSpeed,
                        velocity.y,
                        movement.y * movementSpeed
                    );
                    velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref acceleration, accelerationDuration);
                    velocity = Vector3.SmoothDamp(velocity, dragVelocity, ref dragAcceleration, dragDuration);
                    if (isJumping && Mathf.Approximately(transform.position.y, 0)) {
                        velocity.y = jumpSpeed;
                    }

                    velocity += Physics.gravity * Time.deltaTime;

                    transform.position += velocity * Time.deltaTime;

                    if (transform.position.y < 0) {
                        velocity = new Vector3(velocity.x, 0, velocity.z);
                        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    }
                    break;
                case ControlScheme.Acceleration:
                    acceleration = new Vector3(
                        movement.x * movementSpeed,
                        0,
                        movement.y * movementSpeed
                    );
                    velocity += acceleration * Time.deltaTime;
                    velocity = Vector3.SmoothDamp(velocity, dragVelocity, ref dragAcceleration, dragDuration);

                    if (isJumping && Mathf.Approximately(transform.position.y, 0)) {
                        velocity.y = jumpSpeed;
                    }

                    velocity += Physics.gravity * Time.deltaTime;

                    transform.position += velocity * Time.deltaTime;
                    if (transform.position.y < 0) {
                        velocity = new Vector3(velocity.x, 0, velocity.z);
                        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}