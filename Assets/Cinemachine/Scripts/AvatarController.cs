using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarController : MonoBehaviour {
    [SerializeField]
    CharacterController characterController = default;
    [SerializeField]
    SpriteRenderer attachedRenderer = default;

    [SerializeField, Range(0f, 20f)]
    float maximumSpeed = 5f;
    [SerializeField, Range(0f, 20f)]
    float jumpStartSpeed = 5f;
    [SerializeField, Range(0f, 20f)]
    float jumpStopSpeed = 5f;
    [SerializeField, Range(0f, 10f)]
    float accelerationDuration = 1f;

    PlayerInputActions playerActions;

    public bool isGrounded = false;
    public bool isJumping = false;
    public bool isWalking = false;
    public float currentHorizontalSpeed;
    public float currentVerticalSpeed;


    float currentAcceleration;
    bool jumpButtonWasReleased = true;

    protected void OnEnable() {
        playerActions = new PlayerInputActions();
        playerActions.Enable();
    }

    protected void OnDisable() {
        playerActions.Disable();
    }

    protected void Start() {
        currentAcceleration = 0f;
    }

    protected void Update() {

        isGrounded = characterController.isGrounded;
        float xMovement = playerActions.Player.Move.ReadValue<float>();
        var locationCollider = Vector3.zero;

        bool jumpButtonPressed = playerActions.Player.Jump.phase == InputActionPhase.Performed;
        bool jumpButtonReleased = playerActions.Player.Jump.phase == InputActionPhase.Waiting;


        var tempVelocity = characterController.velocity;
        tempVelocity.x = Mathf.SmoothDamp(tempVelocity.x, xMovement * maximumSpeed, ref currentAcceleration,
            accelerationDuration);

        tempVelocity.y += Physics.gravity.y * Time.deltaTime;

        AdjustSpriteOrientation(xMovement);

        isWalking = false;
        if ((xMovement > 0 || xMovement < 0) && isGrounded) {
            isWalking = true;
        }

        if (jumpButtonPressed && jumpButtonWasReleased && isGrounded) {
            jumpButtonWasReleased = false;
            isJumping = true;
            tempVelocity.y = jumpStartSpeed;
        }

        if (jumpButtonReleased) {
            jumpButtonWasReleased = true;
            if (isJumping) {
                isJumping = false;
                tempVelocity.y = jumpStopSpeed;
            }
        }

        if (tempVelocity.y < jumpStopSpeed) {
            isJumping = false;
        }

        characterController.center = locationCollider;
        currentHorizontalSpeed = tempVelocity.x;
        currentVerticalSpeed = tempVelocity.y;
        characterController.Move(tempVelocity * Time.deltaTime);
    }

    void AdjustSpriteOrientation(float movement) {
        if (movement > 0) {
            attachedRenderer.flipX = false;
        } else if (movement < 0) {
            attachedRenderer.flipX = true;
        }
    }
}