using UnityEngine;

public class CameraMovement:MonoBehaviour {
	public static bool CanRotate { get; set; } = true;
	private float xRotation;
	private float yRotation;
	[SerializeField] private float sensitivity; //Make global for settings later
	[SerializeField] private Transform person;
	private Transform head;

	// Interaction variables
	private Camera mainCamera;
	[SerializeField] private float interactionRange = 5f;
	[SerializeField] private float rayRadius = 0.15f;
	private IInteractable currentTarget;

	[SerializeField] public static KeyCode interactKey = KeyCode.E;


	private void Awake() {
		mainCamera = Camera.main;
		head = transform.parent.gameObject.transform;
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		if(!mainCamera.GetComponent<AudioListener>().enabled)
			mainCamera.GetComponent<AudioListener>().enabled = true;
		if(CanRotate && !PauseMenu.pausedClient) {
			//x-axis controls up and down rotation, y-axis controls left and right rotation.
			yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
			xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

			float headRotation = head.rotation.eulerAngles.x;

			xRotation = Mathf.Clamp(xRotation, headRotation - 50, headRotation + 60);
			transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

			//Rotates the body sideways.
			person.rotation = Quaternion.Euler(0, yRotation, 0);


			RaycastForInteractable();
			if(Input.GetKeyDown(interactKey)) {
				if(currentTarget != null) {
					currentTarget.OnInteract();
				}
			}
		}
	}

	/// <summary>
	/// Draws Gizmos in the Scene view
	/// </summary>
	private void OnDrawGizmos() {
		//Shows the interaction range and size in the scene view for debug
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position + transform.forward * interactionRange, rayRadius);
	}

	/// <summary>
	/// Throws a Spherecast with the <see cref="interactionRange"/> and <see cref="rayRadius"/>
	/// using the <see cref="IInteractable"/> interface.
	/// </summary>
	private void RaycastForInteractable() {
		RaycastHit hitTarget;
		Vector3 start = transform.position;

		if(Physics.CapsuleCast(start, start, rayRadius, transform.forward, out hitTarget, interactionRange)) {
			IInteractable interactable = hitTarget.collider.GetComponent<IInteractable>();

			if(interactable != null) {
				if(hitTarget.distance <= interactable.MaxRange) {
					// We are looking at an interactable within range.
					if(interactable == currentTarget) {
						// We are already looking at this interactable.
						return;
					} else if(currentTarget != null) {
						// We are looking at an interactable.
						currentTarget.OnEndHover();
						currentTarget = interactable;
						currentTarget.OnStartHover();
						return;
					} else {
						// We are looking at an interactable for the first time.
						currentTarget = interactable;
						currentTarget.OnStartHover();
						return;
					}
				} else {
					if(currentTarget != null) {
						// We are looking at an interactable, but it is out of range.
						currentTarget.OnEndHover();
						currentTarget = null;
						return;
					}
				}
			} else {
				// We are looking at something that is not an interactable.
				if(currentTarget != null) {
					currentTarget.OnEndHover();
					currentTarget = null;
					return;
				}
			}
		} else {
			// We are looking at nothing.
			if(currentTarget != null) {
				currentTarget.OnEndHover();
				currentTarget = null;
				return;
			}
		}
	}
}
