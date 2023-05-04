using TMPro;
using UnityEngine;

public class keypadScript:MonoBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	public Door door;
	public TMP_Text answer;
	public GameObject codeLockUI;
	public GameObject keypad;
	public bool tester = false;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		UseKeypad();
		//door.codeLockDoor = false;
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to use keypad";
		Debug.Log(door.code);
	}
	// Start is called before the first frame update
	void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		codeLockUI = transform.parent.parent.GetChild(2).gameObject;
		door = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<Door>();
		keypad = codeLockUI.transform.GetChild(0).gameObject;
		door.codeLockDoor = true;

	}

	// Update is called once per frame
	void Update() {

	}
	private void UseKeypad() {
		ShowKeypadUI();
		answer = codeLockUI.transform.GetChild(0).GetChild(0).GetComponentInChildren<TMP_Text>();
	}

	private void ShowKeypadUI() {
		Cursor.lockState = CursorLockMode.Confined;
		CameraMovement.CanRotate = false;
		keypad.SetActive(true);
		Door.itemText.text = "";
	}

	public void RemoveKeypadUI() {
		keypad.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		CameraMovement.CanRotate = true;
	}
}
