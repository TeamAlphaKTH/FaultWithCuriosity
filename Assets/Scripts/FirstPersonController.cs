using UnityEngine;
using System.Collections;

public class FirstPersonController:MonoBehaviour {
    public bool CanMove { get; private set; } = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 10.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float standingJump = 8f;
    [SerializeField] private float crouchJump = 4f;
    private float jumpForce;

    [Header("Crouching Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 1.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;


    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [SerializeField] private Camera playerCamera;
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
            HandleCrouch();
            ApplyFinalMovement();
        }
    }

    /// <summary>
    /// Handles jump
    /// </summary>
    private void HandleJump() {
        if(canJump) {
            jumpForce = isCrouching ? crouchJump : standingJump;
            if(Input.GetKeyDown(jumpKey) && characterController.isGrounded) {
                moveDirection.y = jumpForce;
            }
        }
    }

    private void HandleCrouch() {
        if(canCrouch) {
            if(Input.GetKey(crouchKey) && !duringCrouchAnimation && characterController.isGrounded) {
                StartCoroutine(CrouchStand());
            }
        }
    }

    private IEnumerator CrouchStand() {
        if(isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f)) {
            yield break;
        }
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetObjectHeight = isCrouching ? characterController.transform.localScale.y * 2 : characterController.transform.localScale.y / 2;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;
        Vector3 newScale = characterController.transform.localScale;
        while(timeElapsed < timeToCrouch) {
            characterController.height = Mathf.Lerp(currentHeight, targetObjectHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            newScale.y = Mathf.Lerp(newScale.y, targetObjectHeight, timeElapsed / timeToCrouch);
            characterController.transform.localScale = newScale;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.transform.localScale = new Vector3(characterController.transform.localScale.x, targetObjectHeight, characterController.transform.localScale.z);
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
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
        /* *** Crouching speed has to be added *** */
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
