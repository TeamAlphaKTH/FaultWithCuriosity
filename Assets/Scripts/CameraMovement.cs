using UnityEngine;

public class CameraMovement:MonoBehaviour {
    public static bool CanRotate { get; set; } = true;
    private float xRotation;
    private float yRotation;
    [SerializeField] private float sensitivity; //Make global for settings later
    [SerializeField] private Transform person;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        if(CanRotate) {
            //x-axis controls up and down rotation, y-axis controls left and right rotation.
            yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            //Rotates the body sideways.
            person.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
