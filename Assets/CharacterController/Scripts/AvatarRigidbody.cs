using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarRigidbody : MonoBehaviour
{
    [SerializeField]
    private Rigidbody attachedRigidbody;
    [SerializeField]
    private ControlScheme scheme = ControlScheme.Velocity;
    [SerializeField, Range(0, 100)]
    private float jumpSpeed = 5;
    [SerializeField, Range(0, 100)]
    private float movementSpeed = 5;
    [SerializeField, Range(0, 10)]
    private float accelerationDuration = 1;
    [SerializeField, Range(0, 100)]
    private float maxPosition = 10;

    [Header("Drag")]
    [SerializeField]
    private Vector3 dragVelocity = Vector3.zero;
    [SerializeField, Range(0, 10)]
    private float dragDuration = 1;
    private Vector3 dragAcceleration;

    private void OnValidate()
    {
        if (!attachedRigidbody)
        {
            TryGetComponent(out attachedRigidbody);
        }
    }

    private Vector2 movement;
    private bool isJumping;
    [SerializeField]
    private Vector3 velocity;
    private Vector3 acceleration;

    private void Update()
    {
        movement = Gamepad.current.leftStick.ReadValue();
        isJumping = Gamepad.current.aButton.isPressed;
    }

    private void FixedUpdate()
    {
        switch (scheme)
        {
            case ControlScheme.Position:
                break;
            case ControlScheme.Velocity:
                Vector3 targetVelocity = new Vector3(
                    movement.x * movementSpeed,
                    attachedRigidbody.velocity.y,
                    movement.y * movementSpeed
                );
                attachedRigidbody.velocity = Vector3.SmoothDamp(attachedRigidbody.velocity, targetVelocity, ref acceleration, accelerationDuration);
                attachedRigidbody.velocity = Vector3.SmoothDamp(attachedRigidbody.velocity, dragVelocity, ref dragAcceleration, dragDuration);
                if (isJumping && Mathf.Approximately(transform.position.y, 0))
                {
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

    private void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent<DragSource>(out DragSource drag))
        {
            dragVelocity = drag.velocity;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        dragVelocity = Vector3.zero;
    }

    private void AddForce(Vector3 value, ForceMode mode)
    {
        switch (mode)
        {
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
