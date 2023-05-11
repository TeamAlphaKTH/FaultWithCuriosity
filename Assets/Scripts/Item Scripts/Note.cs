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

	//Sound
	[Header("Audio")]
	[SerializeField] private AudioSource noteAudio;

	// Change this to false if you want to use a note without a code lock.
	[SerializeField] private bool isCodeLock = true;

	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		itemText.text = "";
		// Set the code text to the code of the door
		if(isCodeLock) {
			codeText.text = "CODE: \n " + door.code.Value;
		} else {
			codeText.text = noteUI.GetComponentInChildren<TMP_Text>().text;
		}

		// Activate the note UI and disable movement and camera. 
		isOn = !isOn;
		noteAudio.Play();
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
		if(isCodeLock) {
			door = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<Door>();
		}

		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();

		noteUI = transform.parent.GetChild(1).GetComponent<Canvas>().gameObject;
		codeText = noteUI.GetComponentInChildren<TMP_Text>();
	}
	void Update() {
		if(isOn && Input.GetKeyDown(KeyCode.R)) {
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
