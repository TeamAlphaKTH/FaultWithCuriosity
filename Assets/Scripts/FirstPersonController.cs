using UnityEngine;
public class FirstPersonController:MonoBehaviour {
    public bool CanMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float gravity = 30.0f;

    private Vector2 currentInput;
    private Vector3 moveDirection;
    private CharacterController characterController;

    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {

        if(CanMove) {
            HandleInput();
            ApplyFinalMovements();
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
    /// Applies the final movements to the character.
    /// </summary>
    /// <remarks>
    /// If the character is not grounded, applies gravity to the movement direction.
    /// Then, moves the character controller according to the final movement direction.
    /// </remarks>
    void ApplyFinalMovements() {
        if(!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
