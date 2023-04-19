using UnityEngine;

public class FirstPersonController:MonoBehaviour {
    public bool CanMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 10.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;

    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Vector2 currentInput;
    private Vector3 moveDirection;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if(CanMove) {
            HandleInput();
            HandleJump();
            ApplyFinalMovement();
        }
    }

    /// <summary>
    /// Handles jump
    /// </summary>
    private void HandleJump() {
        if(canJump) {
            if(Input.GetKey(jumpKey) && characterController.isGrounded) {
                moveDirection.y = jumpForce;
            }
        }
    }

    /// <summary>
    /// Handles the input for the character movement.
    /// </summary>
    /// <remarks>
    /// Calculates the movement direction based on the player's input and sets the character's velocity accordingly.
    /// </remarks>
    void HandleInput() {

        //2D vector based on the player's input axis for vertical and horizontal movement and scales it by walk speed.
        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;

        //Calculates the movement direction of the character based on the current input vector and the orientation of the character in the world.
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    /// <summary>
    /// Applies movement to player, depending on positional values
    /// </summary>
    private void ApplyFinalMovement() {
        if(!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
