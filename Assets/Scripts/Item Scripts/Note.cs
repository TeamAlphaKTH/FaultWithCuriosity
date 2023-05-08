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
		codeText.text = "CODE: \n " + door.code.Value;
		isOn = !isOn;
		noteUI.SetActive(true);
		CameraMovement.CanRotate = false;
		PhotoCapture.canUseCamera = false;
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to read note";
	}


	// Start is called before the first frame update
	void Start() {
		door = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<Door>();

		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();

		noteUI = transform.parent.GetChild(1).GetComponent<Canvas>().gameObject;
		codeText = noteUI.GetComponentInChildren<TMP_Text>();

	}
	void Update() {
		if(isOn && Input.GetKeyDown(KeyCode.Escape)) {
			noteUI.SetActive(false);
			CameraMovement.CanRotate = true;
			PhotoCapture.canUseCamera = true;
			Cursor.lockState = CursorLockMode.Locked;
			isOn = false;
			OnStartHover();
		}
	}
}
