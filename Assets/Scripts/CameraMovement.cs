using Unity.Netcode;
using UnityEngine;

public class CameraMovement:NetworkBehaviour {
	public static bool CanRotate { get; set; } = true;
	private float xRotation;
	private float yRotation;
	[SerializeField] private float sensitivity; //Make global for settings later
	[SerializeField] private Transform person;

	// Interaction variables
	private Camera mainCamera;
	[SerializeField] private float interactionRange = 5f;
	private IInteractable currentTarget;

	[SerializeField] public static KeyCode interactKey = KeyCode.E;


	private void Awake() {
		mainCamera = Camera.main;
	}

	// Start is called before the first frame update
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update() {
		if(!IsOwner)
			return;
		if(CanRotate && !PauseMenu.pausedClient) {
			//x-axis controls up and down rotation, y-axis controls left and right rotation.
			yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
			xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

			xRotation = Mathf.Clamp(xRotation, -90, 90);
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


	private void RaycastForInteractable() {
		RaycastHit hitTarget;

		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out hitTarget, interactionRange)) {
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

