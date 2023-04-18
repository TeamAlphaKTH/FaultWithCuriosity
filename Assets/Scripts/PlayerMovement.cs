using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            HandleJump();
    }

    /// <summary>
    /// Returns true if player is on the ground
    /// </summary>
    private bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f);
	}

    /// <summary>
    /// Checks if spacebar has been pressed AND the player is grounded, then the player jumps
    /// </summary>
    private void HandleJump() {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
		}
	}
}
