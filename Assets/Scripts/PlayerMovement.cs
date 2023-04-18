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

    // Update is called once per frame
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
        rb.velocity = direction * walkSpeed;
    }
}
