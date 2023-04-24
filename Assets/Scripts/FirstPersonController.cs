using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class FirstPersonController:NetworkBehaviour {

    public bool CanMove { get; private set; } = true;
    private bool IsRunning => Input.GetKey(runKey) && canRun;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float climbSpeed = 3.0f;
    [SerializeField] private float slopeSlideSpeed = 8.0f;

    // Movement speed when the player is not moving forward.
    [SerializeField] private float slowSpeedWalk = 1.8f;
    [SerializeField] private float slowSpeedRun = 3.6f;

    // Lower values will make the character accelerate slower, higher values will make the character accelerate faster.
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float deceleration = 100f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 40f;
    [SerializeField] private float jumpForce = 7.8f;
    [SerializeField] private float standingJump = 8f;
    [SerializeField] private float crouchJump = 4f;

    [Header("Crouching Parameters")]
    [SerializeField] private float crouchHeight;
    [SerializeField] private float standingHeight;
    [SerializeField] private Vector3 crouchingCenter;
    [SerializeField] private Vector3 standingCenter;
    private float timeToCrouch = 0.25f;
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canRun = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool willSlideOnSlope = true;

    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    // Slope sliding parameters
    private Vector3 hitPointNormal;
    private bool IsSliding {
        get {
            // Check if characther is grounded and raycast hit a slope
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)) {
                // Check if slope angle is greater than character controller's slope limit
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            } else {
                return false;
            }
        }
    }

    private Vector2 currentInput;

    // Initialize currentSpeed to zero so that the character doesn't move when the game starts.
    private Vector2 currentSpeed = Vector2.zero;
    private Vector3 moveDirection;
    private CharacterController characterController;
    private float oldGravity;

    [Header("Camera")]
    private Camera playerCamera;
    [SerializeField] private float cameraPos = -0.7f;

    public override void OnNetworkSpawn() {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        standingCenter = characterController.center;
        standingHeight = characterController.height;
        Initialize();
        base.OnNetworkSpawn();
    }

    // Start is called before the first frame update
    void Initialize() {
        if(!IsOwner) { playerCamera.enabled = false; }
        crouchHeight = standingHeight / 2;
        crouchingCenter = standingCenter / 2;
    }

    // Update is called once per frame
    void Update() {
        if(!IsOwner)
            return;

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
            if(Input.GetKey(jumpKey) && characterController.isGrounded && !IsSliding) {
                moveDirection.y = jumpForce;
            }
        }
    }

    private void HandleCrouch() {
        if(canCrouch) {
            if(Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded) {
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
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;

        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;
        while(timeElapsed < timeToCrouch) {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.position += new Vector3(0, cameraPos, 0);
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        cameraPos = -cameraPos;
        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }


    /// <summary>
    /// Handles the input for the character movement.
    /// </summary>
    /// <remarks>
    /// Calculates the movement direction based on the player's input and sets the character's velocity accordingly.
    /// Lets the player accelerate when it wants to move. When no input is given, the character will stop moving.
    /// </remarks>
    private void HandleInput() {
        // If the player is running, the walk speed is set to the run speed, otherwise it is set to the walk speed.
        float baseSpeed = IsRunning && !IsSliding ? runSpeed : walkSpeed;
        float speed = baseSpeed;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float accelerationRate = horizontalInput == 0 && verticalInput == 0 ? deceleration : acceleration;

        // If the player is not moving forward, use slower speed.
        if(horizontalInput == 0 || verticalInput == 0) {
            if(horizontalInput < 0 || horizontalInput > 0 || verticalInput < 0) {
                // Moving backwards or sideways, use slower speed.
                speed = IsRunning ? slowSpeedRun : slowSpeedWalk;
            } else {
                speed = baseSpeed;
            }
        } else {
            // Moving diagonally, use slower speed, calculated with pythagorean theorem.
            float diagonalSpeed = Mathf.Sqrt(baseSpeed * baseSpeed / 2f);

            if(verticalInput < 0) {
                // Moving diagonally backward, use slower diagonal speed.
                speed = IsRunning ? slowSpeedRun * diagonalSpeed / baseSpeed : slowSpeedWalk * diagonalSpeed / baseSpeed;
            } else {
                // Moving diagonally forward, use normal diagonal speed.
                speed = diagonalSpeed;
            }
        }

        // 2D vector based on the player's input axis for vertical and horizontal movement and scales it by walk speed.
        currentInput = new Vector2(speed * verticalInput, speed * horizontalInput);

        currentSpeed = Vector2.MoveTowards(currentSpeed, currentInput, accelerationRate * Time.deltaTime);
        Debug.Log("Current speed: " + currentSpeed);
        float moveDirectionY = moveDirection.y;

        // Calculates the movement direction of the character based on the current input vector and the orientation of the character in the world.
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentSpeed.x) + (transform.TransformDirection(Vector3.right) * currentSpeed.y);
        moveDirection.y = moveDirectionY;
    }

    /// <summary>
    /// Applies movement to player, depending on positional values
    /// </summary>
    private void ApplyFinalMovement() {
        // Apply gravity
        if(!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        // Apply slope sliding
        if(willSlideOnSlope && IsSliding) {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSlideSpeed;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Handles player on ladder going up down left and right 
    /// </summary>
    /// <param name="ladderHitbox"></param>
    private void OnTriggerStay(Collider ladderHitbox) {
        //Check that the gameobject is a "Ladder"
        if(ladderHitbox.CompareTag("Ladder")) {
            //Turn off gravity and normal movement while on "Ladder"
            if(gravity != 0f)
                oldGravity = gravity;

            gravity = 0f;

            if(!characterController.isGrounded) {
                CanMove = false;
            } else {
                CanMove = true;
            }

            //Handles movement on "Ladder"
            if(Input.GetAxis("Vertical") > 0) {
                Debug.Log(Input.GetAxis("Vertical"));
                moveDirection = (transform.TransformDirection(Vector3.up) * climbSpeed);
                characterController.Move(moveDirection * Time.deltaTime);
            } else if(Input.GetAxis("Vertical") < 0) {
                moveDirection = (transform.TransformDirection(Vector3.down) * climbSpeed);
                characterController.Move(moveDirection * Time.deltaTime);
            }
            if(Input.GetAxis("Horizontal") > 0) {
                moveDirection = (transform.TransformDirection(Vector3.right) * climbSpeed);
                characterController.Move(moveDirection * Time.deltaTime);
            } else if(Input.GetAxis("Horizontal") < 0) {
                moveDirection = (transform.TransformDirection(Vector3.left) * climbSpeed);
                characterController.Move(moveDirection * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Handles when player leave ladder
    /// </summary>
    /// <param name="ladderHitbox"></param>
    private void OnTriggerExit(Collider ladderHitbox) {
        if(ladderHitbox.CompareTag("Ladder")) {
            //Activates normal movement and gravity
            CanMove = true;
            gravity = oldGravity;
        }
    }
}
