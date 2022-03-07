using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarCharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterController attachedCharacter;
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
        if (!attachedCharacter)
        {
            TryGetComponent(out attachedCharacter);
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
        dragVelocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(transform.position, attachedCharacter.radius, -1, QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<DragSource>(out DragSource drag))
                {
                    dragVelocity = drag.velocity;
                    break;
                }
            }
        }

        switch (scheme)
        {
            case ControlScheme.Position:
                break;
            case ControlScheme.Velocity:
                Vector3 targetVelocity = new Vector3(
                    movement.x * movementSpeed,
                    velocity.y,
                    movement.y * movementSpeed
                );
                velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref acceleration, accelerationDuration);
                velocity = Vector3.SmoothDamp(velocity, dragVelocity, ref dragAcceleration, dragDuration);
                if (isJumping && attachedCharacter.isGrounded)
                {
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
