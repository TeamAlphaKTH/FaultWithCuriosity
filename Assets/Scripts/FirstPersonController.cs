using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPersonController:NetworkBehaviour {

	public static bool CanMove { get; set; } = true;
	private bool IsRunning => Input.GetKey(runKey) && canRun;

	[Header("Enemy")]
	[SerializeField] private GameObject enemyGhostPrefab;
	[SerializeField] private GameObject enemySpawnPoints;

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

	[Header("Functional Options")]
	[SerializeField] private bool canJump = true;
	[SerializeField] private bool canRun = true;
	[SerializeField] private bool canCrouch = true;
	[SerializeField] private bool willSlideOnSlope = true;

	[Header("Jumping Parameters")]
	[SerializeField] private float gravity = 40f;
	[SerializeField] private float jumpForce = 7.8f;
	[SerializeField] private float standingJump = 8f;
	[SerializeField] private float crouchJump;

	[Header("Camera Reference")]
	[SerializeField] public Transform playerCamera;
	Vector3 initialCameraPosition;

	[Header("Crouching Parameters")]
	[SerializeField] private float crouchMultiplier = 0.6f;
	// All these parameters are Seralized for testing purposes - their value is set in start()
	[SerializeField] private float crouchHeight;
	[SerializeField] private float standingHeight;
	[SerializeField] private Vector3 crouchingCenter;
	[SerializeField] private Vector3 standingCenter;

	// Changes the time between toggle and hold crouch - must be above 0.15f
	private float timeToCrouch = 0.25f;
	private bool isCrouching;
	private bool duringCrouchAnimation;

	[Header("Stamina system parameters")]
	[SerializeField] private float maxStamina = 100.0f;
	[SerializeField] private float staminaRegenIncrements = 1.0f;
	[SerializeField] private float staminaUseDecrements = 15.0f;
	[SerializeField] private float staminaRegenDelayTimer = 3.0f;
	[SerializeField] private float speedToRegen = 0.2f;
	[SerializeField] private float currentStamina;
	[SerializeField] private Slider staminaSlider;
	private Coroutine regeneratingStamina;
	private bool useStamina = true;
	public static Action<float> OnStaminaChange;

	[Header("Controls")]
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode runKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode toggleCrouchKey = KeyCode.C;
	[SerializeField] public static KeyCode useCameraButton = KeyCode.Mouse1;
	[SerializeField] public static KeyCode openInventory = KeyCode.Tab;

	[Header("Audio")]
	[SerializeField] private AudioClip[] walkClips;
	[SerializeField] private AudioSource myAudioSource;
	private int pickSound;
	[SerializeField] private AudioClip[] breathingClips;
	[SerializeField] private AudioSource breathingSource;

	[Header("Animations")]
	[SerializeField] private Animator animator;

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
	public static CharacterController characterController;
	private float oldGravity;

	// Is called first
	public override void OnNetworkSpawn() {
		if(!IsOwner) {
			GetComponentInChildren<Camera>().enabled = false;
			return;
		}
		characterController = GetComponent<CharacterController>();

		staminaSlider = GameObject.Find("Stamina Slider").GetComponent<Slider>();
		standingCenter = characterController.center;
		standingHeight = characterController.height;
		Initialize();
		base.OnNetworkSpawn();
	}

	private void Start() {
		if(IsOwner) {
			SpawnEnemy();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	void Initialize() {
		if(!IsOwner) {
			return;
		}
		// for crouching
		initialCameraPosition = playerCamera.transform.localPosition;
		crouchHeight = standingHeight * crouchMultiplier;
		crouchingCenter = standingCenter * crouchMultiplier;
		crouchJump = standingJump * crouchMultiplier;

		currentStamina = maxStamina;
		staminaSlider.value = maxStamina;

	}

	private void SpawnEnemy() {
		Transform[] enemySpawn = enemySpawnPoints.GetComponentsInChildren<Transform>();
		Instantiate(enemyGhostPrefab, enemySpawn[1].position, Quaternion.identity);
		EnemyController.player = NetworkManager.LocalClient.PlayerObject.transform;
	}

	// Update is called once per frame
	void Update() {
		if(!IsOwner) {
			return;
		}
		if(CanMove && !PauseMenu.pausedClient) {
			HandleInput();
			HandleJump();
			HandleCrouch();
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
			jumpForce = isCrouching ? crouchJump : standingJump;
			if(Input.GetKey(jumpKey) && characterController.isGrounded && !IsSliding) {
				moveDirection.y = jumpForce;
			}
		}
	}

	/// <summary>
	/// Handles the character crouch.
	/// </summary>
	private void HandleCrouch() {
		if(canCrouch) {
			// Both toggle coruch and hold crouch with || Input.GetKeyUp(crouchKey)  
			if(Input.GetKeyDown(toggleCrouchKey) && !duringCrouchAnimation && characterController.isGrounded) {
				StartCoroutine(CrouchStand());
			}
		}
	}

	private IEnumerator CrouchStand() {
		// Can't stand when blocking object (1f up) is above player
		// won't stand up when hold crouch is released - changes automatically to toggle crouch
		if(isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f)) {
			yield break;
		}
		duringCrouchAnimation = true;

		float timeElapsed = 0;
		float targetHeight = isCrouching ? standingHeight : crouchHeight;
		float currentHeight = characterController.height;

		Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
		Vector3 currentCenter = characterController.center;

		// Above while loop for hold crouch 
		isCrouching = !isCrouching;

		CrouchServerRpc(isCrouching);

		// Changes the characters hitbox / collider
		while(timeElapsed < timeToCrouch) {
			characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
			characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		// Ensure correct moved charachter collider
		characterController.height = targetHeight;
		characterController.center = targetCenter;

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

		if(isCrouching) {
			speed *= crouchMultiplier;
		}

		// 2D vector based on the player's input axis for vertical and horizontal movement and scales it by walk speed.
		currentInput = new Vector2(speed * verticalInput, speed * horizontalInput);

		currentSpeed = Vector2.MoveTowards(currentSpeed, currentInput, accelerationRate * Time.deltaTime);
		float moveDirectionY = moveDirection.y;

		// Calculates the movement direction of the character based on the current input vector and the orientation of the character in the world.
		moveDirection = (transform.TransformDirection(Vector3.forward) * currentSpeed.x) + (transform.TransformDirection(Vector3.right) * currentSpeed.y);
		moveDirection.y = moveDirectionY;

		MovementServerRpc(currentSpeed.magnitude);
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

		// sound for walking
		if(characterController.isGrounded && moveDirection.x != 0 && moveDirection.z != 0) {
			pickSound = UnityEngine.Random.Range(0, walkClips.Length);
			if(!myAudioSource.isPlaying) {
				myAudioSource.clip = walkClips[pickSound];
				myAudioSource.Play();
			}
		}

		// sound for breathing
		if(!breathingSource.isPlaying) {
			if(currentStamina < 80 && currentStamina > 30) {
				breathingSource.clip = breathingClips[0];
				breathingSource.Play();
			} else if(currentStamina <= 30 && currentStamina >= 0) {
				breathingSource.clip = breathingClips[1];
				breathingSource.Play();
			}
		}
	}

	/// <summary>
	/// Handles player on ladder going up down left and right 
	/// </summary>
	/// <param name="ladderHitbox"></param>
	private void OnTriggerStay(Collider ladderHitbox) {
		if(!IsOwner) {
			return;
		}
		//Check that the gameobject is a "Ladder"
		if(ladderHitbox.CompareTag("Ladder")) {
			//Turn off gravity and normal movement while on "Ladder"
			if(gravity != 0f)
				oldGravity = gravity;

			gravity = 0f;

			if(!characterController.isGrounded) {
				CanMove = false;
				ClimbServerRpc(true);
			} else {
				CanMove = true;
			}

			//Handles movement on "Ladder"
			if(Input.GetAxis("Vertical") > 0) {
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
			HandleStamina();
		}
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
			if(currentStamina <= 0) {
				currentStamina = 0;
				canRun = false;
			}

			// Cap the current stamina to the maximum stamina value.
			else if(currentStamina > 100) {
				currentStamina = 100;
			}

			// Invoke the OnStaminaChange event to notify any listeners that the player's stamina has changed.
			OnStaminaChange?.Invoke(currentStamina);
		}

		// Start regenerating stamina if the player is not running and their current stamina is less than the maximum stamina.
		if(!IsRunning && currentStamina < maxStamina && regeneratingStamina == null) {
			regeneratingStamina = StartCoroutine(RegenStamina());
		}
		staminaSlider.value = currentStamina;
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

	/// <summary>
	/// Handles when player leave ladder
	/// </summary>
	/// <param name="ladderHitbox"></param>
	private void OnTriggerExit(Collider ladderHitbox) {
		if(!IsOwner) {
			return;
		}
		if(ladderHitbox.CompareTag("Ladder")) {
			//Activates normal movement and gravity
			CanMove = true;
			gravity = oldGravity;
			ClimbServerRpc(false);
		}
	}

	private void OnTriggerEnter(Collider winningHitbox) {
		if(winningHitbox.CompareTag("Win"))
			winServerRpc();
	}
	[ServerRpc(RequireOwnership = false)]
	private void winServerRpc() {
		winClientRpc();
	}
	[ClientRpc]
	private void winClientRpc() {
		NetworkManager.SceneManager.LoadScene("Credits", LoadSceneMode.Single);
	}

	[ServerRpc]
	private void MovementServerRpc(float speed) {
		MovementClientRpc(speed);
	}

	[ClientRpc]
	private void MovementClientRpc(float speed) {
		animator.SetFloat("Movement", speed);
	}

	[ServerRpc]
	private void CrouchServerRpc(bool crouch) {
		CrouchClientRpc(crouch);
	}

	[ClientRpc]
	private void CrouchClientRpc(bool crouch) {
		animator.SetBool("Crouch", crouch);
	}

	[ServerRpc]
	private void ClimbServerRpc(bool climb) {
		ClimbClientRpc(climb);
	}

	[ClientRpc]
	private void ClimbClientRpc(bool climb) {
		animator.SetBool("Climb", climb);
	}

}
