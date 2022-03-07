using UnityEngine;
using UnityEngine.InputSystem;

namespace AGGE.CharacterController {
    public class AvatarRigidbody : MonoBehaviour {
        [SerializeField]
        Rigidbody attachedRigidbody;
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
            if (!attachedRigidbody) {
                TryGetComponent(out attachedRigidbody);
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
            switch (scheme) {
                case ControlScheme.Position:
                    break;
                case ControlScheme.Velocity:
                    var targetVelocity = new Vector3(
                        movement.x * movementSpeed,
                        attachedRigidbody.velocity.y,
                        movement.y * movementSpeed
                    );
                    attachedRigidbody.velocity = Vector3.SmoothDamp(attachedRigidbody.velocity, targetVelocity, ref acceleration, accelerationDuration);
                    attachedRigidbody.velocity = Vector3.SmoothDamp(attachedRigidbody.velocity, dragVelocity, ref dragAcceleration, dragDuration);
                    if (isJumping && Mathf.Approximately(transform.position.y, 0)) {
                        attachedRigidbody.velocity = new Vector3(
                            attachedRigidbody.velocity.x,
                            jumpSpeed,
                            attachedRigidbody.velocity.z
                        );
                    }
                    break;
                case ControlScheme.Acceleration:
                    //attachedRigidbody.AddForce(Physics.gravity, ForceMode.);
                    break;
                default:
                    break;
            }
        }

        void OnTriggerStay(Collider collider) {
            if (collider.TryGetComponent<DragSource>(out var drag)) {
                dragVelocity = drag.velocity;
            }
        }
        void OnTriggerExit(Collider other) {
            dragVelocity = Vector3.zero;
        }

        void AddForce(Vector3 value, ForceMode mode) {
            switch (mode) {
                case ForceMode.VelocityChange:
                    // [v] = m / s
                    attachedRigidbody.velocity += value;
                    return;
                case ForceMode.Acceleration:
                    // [a] = m / s²
                    AddForce(value * Time.deltaTime, ForceMode.VelocityChange);
                    return;
                case ForceMode.Impulse:
                    // [I] = m * kg / s
                    AddForce(value / attachedRigidbody.mass, ForceMode.VelocityChange);
                    return;
                case ForceMode.Force:
                    // [F] = m * kg / s²
                    AddForce(value * Time.deltaTime / attachedRigidbody.mass, ForceMode.VelocityChange);
                    return;
            }
        }
    }
}