using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
	[SerializeField] private CharacterController characterController;

	[Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Functional Options")]

    [SerializeField] private bool canJump = true;

    [Header("Controls")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove) {
			HandleJump();
			ApplyFinalMovement();
		}
    }

    /// <summary>
    /// Handles jump
    /// </summary>
    private void HandleJump() {
        if (canJump) {
			if(Input.GetKey(jumpKey) && characterController.isGrounded) {
				moveDirection.y = jumpForce;
			}
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
}
