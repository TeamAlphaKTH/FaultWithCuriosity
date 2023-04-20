using System;
using System.Collections;
using UnityEngine;

public class FirstPersonController:MonoBehaviour {
    public bool CanMove { get; private set; } = true;
    private bool IsRunning => Input.GetKey(runKey) && canRun;

    private bool useStamina = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float climbSpeed = 3.0f;

    // Movement speed when the player is not moving forward.
    [SerializeField] private float slowSpeedWalk = 1.8f;
    [SerializeField] private float slowSpeedRun = 3.6f;

    // Lower values will make the character accelerate slower, higher values will make the character accelerate faster.
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float deceleration = 100f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Functional Options")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canRun = true;

    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [Header("Stamina system parameters")]
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float staminaRegenIncrements = 1.0f;
    [SerializeField] private float staminaUseDecrements = 15.0f;
    [SerializeField] private float staminaRegenDelayTimer = 3.0f;
    [SerializeField] private float speedToRegen = 0.2f;
    [SerializeField] private float regenTimer = 0.01f;
    [SerializeField] private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;


    private Vector2 currentInput;

    // Initialize currentSpeed to zero so that the character doesn't move when the game starts.
    private Vector2 currentSpeed = Vector2.zero;
    private Vector3 moveDirection;
    private CharacterController characterController;
    private float oldGravity;

    // Start is called before the first frame update
    void Start() {
        characterController = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update() {
        if(CanMove) {
            HandleInput();
            HandleJump();
            ApplyFinalMovement();
            if(useStamina) {
                HandleStamina();
            }
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
    /// Lets the player accelerate when it wants to move. When no input is given, the character will stop moving.
    /// </remarks>
    private void HandleInput() {
        // If the player is running, the walk speed is set to the run speed, otherwise it is set to the walk speed.
        float baseSpeed = IsRunning ? runSpeed : walkSpeed;
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
    /// Handles the management of the player's stamina.
    /// </summary>
    private void HandleStamina() {
        if(IsRunning && currentInput != Vector2.zero) {

            if(regeneratingStamina != null) {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            // Decrease the current stamina based on the stamina use decrements and the time passed since the last frame.
            currentStamina -= staminaUseDecrements * Time.deltaTime;
            if(currentStamina < 0) {
                currentStamina = 0;
            }

            // Invoke the OnStaminaChange event to notify any listeners that the player's stamina has changed.
            OnStaminaChange?.Invoke(currentStamina);

            // Disable running if the current stamina has reached zero
            if(currentStamina <= 0) {
                canRun = false;
            }

            // Cap the current stamina to the maximum stamina value.
            if(currentStamina > 100) {
                currentStamina = 100;
            }
        }

        // Start regenerating stamina if the player is not running and their current stamina is less than the maximum stamina.
        if(!IsRunning && currentStamina < maxStamina && regeneratingStamina == null) {
            regeneratingStamina = StartCoroutine(RegenStamina());
        }
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

    /// <summary>
    /// Coroutine that regenerates the player's stamina over time.
    /// </summary>
    private IEnumerator RegenStamina() {

        // Wait for the specified amount of time before starting to regenerate stamina.
        yield return new WaitForSeconds(staminaRegenDelayTimer);
        WaitForSeconds TimeToWait = new(speedToRegen);

        while(currentStamina < maxStamina) {

            // Enable running if the player's stamina is greater than zero.
            if(currentStamina > 0) {
                canRun = true;
            }

            // Increase the current stamina based on the stamina regen increments.
            currentStamina += staminaRegenIncrements;

            // Cap the current stamina to the maximum stamina value.
            if(currentStamina > maxStamina) {
                currentStamina = maxStamina;
            }

            // Invoke the OnStaminaChange event to notify any listeners that the player's stamina has changed.
            OnStaminaChange?.Invoke(currentStamina);

            // Wait for the specified amount of time before regenerating stamina again.
            yield return TimeToWait;
        }

        // Reset the regeneratingStamina coroutine to null once the player's stamina has fully regenerated.
        regeneratingStamina = null;
    }
}
