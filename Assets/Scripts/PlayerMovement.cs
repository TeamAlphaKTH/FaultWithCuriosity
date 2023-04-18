using UnityEngine;

public class PlayerMovement:MonoBehaviour {
    [SerializeField] private float walkSpeed = 1.0f;
    public Rigidbody rb;

    // Direction of the player based on Update() input.
    public Vector3 movement;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    /* Update is called once per frame, used when you want to check for input.
     * FixedUpdate is called once per physics update, used when you want to move the player.
     * Ground check is supposed to be in Update, want to check it every frame. 
     */
    void Update() {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// Used for physics movement.
    /// </summary>
    private void FixedUpdate() {
        movePlayer(movement);
    }

    /// <summary>
    /// Moves the player in the given direction.
    /// </summary>
    /// <param name="direction">Direction of the player as a <see cref="Vector3"/></param>
    void movePlayer(Vector3 direction) {
        rb.velocity = Time.deltaTime * walkSpeed * direction;
    }
}
