using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarTransform : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private float jumpSpeed = 5;

    [SerializeField]
    private ControlScheme scheme = ControlScheme.Velocity;

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

    private Vector2 movement;
    [SerializeField]
    private Vector3 velocity;
    private Vector3 acceleration;
    private bool isJumping;

    private void Update()
    {
        movement = Gamepad.current.leftStick.ReadValue();
        isJumping = Gamepad.current.aButton.isPressed;
    }

    private void FixedUpdate()
    {
        dragVelocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, -1, QueryTriggerInteraction.Collide);
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
                transform.position = new Vector3(
                    movement.x * maxPosition,
                    transform.position.y,
                    movement.y * maxPosition
                );
                break;
            case ControlScheme.Velocity:
                Vector3 targetVelocity = new Vector3(
                    movement.x * movementSpeed,
                    velocity.y,
                    movement.y * movementSpeed
                );
                velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref acceleration, accelerationDuration);
                velocity = Vector3.SmoothDamp(velocity, dragVelocity, ref dragAcceleration, dragDuration);
                if (isJumping && Mathf.Approximately(transform.position.y, 0))
                {
                    velocity.y = jumpSpeed;
                }

                velocity += Physics.gravity * Time.deltaTime;

                /*
                float distance = (velocity * Time.deltaTime).magnitude;
                
                if (Physics.BoxCast(transform.position, transform.localScale / 2, velocity, transform.rotation, distance))
                {
                    Debug.Log("we won't move because we would've collided with something");
                }
                else
                {
                }
                //*/
                transform.position += velocity * Time.deltaTime;

                if (transform.position.y < 0)
                {
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

                if (isJumping && Mathf.Approximately(transform.position.y, 0))
                {
                    velocity.y = jumpSpeed;
                }

                velocity += Physics.gravity * Time.deltaTime;

                transform.position += velocity * Time.deltaTime;
                if (transform.position.y < 0)
                {
                    velocity = new Vector3(velocity.x, 0, velocity.z);
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
                break;
            default:
                break;
        }
    }
}
