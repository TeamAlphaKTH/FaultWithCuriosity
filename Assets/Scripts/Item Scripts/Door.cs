using TMPro;
using UnityEngine;

public class Door:MonoBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;

	public static string code;

	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		Keypad.UseKeypad();
		if(!animator.GetBool("OpenDoor") && Keypad.canOpenDoor) {
			animator.SetBool("OpenDoor", true);
			animator.SetBool("CloseDoor", false);
		} else {
			animator.SetBool("OpenDoor", false);
			animator.SetBool("CloseDoor", true);
		}
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to use door";
		Debug.Log(code);
	}
	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();
		code = GenerateCode();
	}

	private string GenerateCode() {
		int randomNumber = Random.Range(0, 10000);
		string code = randomNumber.ToString("D4");
		return code;
	}
}
