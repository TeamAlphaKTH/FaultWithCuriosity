using TMPro;
using UnityEngine;

public class Note:MonoBehaviour, IInteractable {
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 2f;

	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	[SerializeField] private GameObject noteUI;
	private bool isOn = false;
	private TMP_Text codeText;
	public Door door;
	public keypadScript keypadScript;

	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		itemText.text = "";
		// Set the code text to the code of the door
		if(transform.parent.parent != null) {
			codeText.text = "CODE: \n " + door.code.Value;
		} else {
			codeText.text = null;
		}

		// Activate the note UI and disable movement and camera. 
		isOn = !isOn;
		noteUI.SetActive(true);
		CameraMovement.CanRotate = false;
		FirstPersonController.CanMove = false;
		PhotoCapture.canUseCamera = false;
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to read note";
	}


	// Start is called before the first frame update
	void Start() {
		// Get door component.
		if(transform.parent.parent != null) {
			door = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<Door>();
		}

		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();

		noteUI = transform.parent.GetChild(1).GetComponent<Canvas>().gameObject;
		codeText = noteUI.GetComponentInChildren<TMP_Text>();

	}
	void Update() {
		if(isOn && Input.GetKeyDown(KeyCode.Escape)) {
			// If the note is on and the player presses escape, disable the note UI and enable movement and camera.
			noteUI.SetActive(false);
			CameraMovement.CanRotate = true;
			PhotoCapture.canUseCamera = true;
			FirstPersonController.CanMove = true;
			isOn = false;
			OnStartHover();
		}
	}
}
