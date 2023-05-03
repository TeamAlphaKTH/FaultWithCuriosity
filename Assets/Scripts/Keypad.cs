using TMPro;
using UnityEngine;

public class Keypad:MonoBehaviour {
	public static TMP_Text answer;
	public static GameObject codeLockUI;
	public static GameObject keypad;

	public Door instanceDoor;

	void Start() {
		codeLockUI = GameObject.Find("CodeLockUI").GetComponentInChildren<Canvas>().gameObject;
		keypad = codeLockUI.transform.GetChild(0).gameObject;
		instanceDoor = GameObject.Find("Key Id").GetComponentInChildren<Door>();
	}

	/// <summary>
	/// Inputs the number.
	/// </summary>
	/// <param name="number">The number.</param>
	public void InputNumber(int number) {
		if(answer.text.Length < 4) {
			answer.text += number.ToString();
		}
	}

	public void CheckCode() {
		if(answer.text.Equals(Door.code)) {
			instanceDoor.canOpenDoor = true;
			answer.text = "Correct";
			RemoveKeypadUI();
			Door.itemText.text = "Press " + CameraMovement.interactKey + " to use door";
		} else {
			answer.text = "Incorrect";
		}
	}

	public static void UseKeypad() {
		ShowKeypadUI();
		answer = GameObject.Find("Input Text").GetComponentInChildren<TMP_Text>();
	}

	public static void Delete() {
		answer.text = "";
	}

	public static void ShowKeypadUI() {
		Cursor.lockState = CursorLockMode.Confined;
		CameraMovement.CanRotate = false;
		keypad.SetActive(true);
		Door.itemText.text = "";
	}

	public static void RemoveKeypadUI() {
		Delete();
		keypad.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		CameraMovement.CanRotate = true;
	}
}
