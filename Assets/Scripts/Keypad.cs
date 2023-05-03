using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Keypad:MonoBehaviour {
	public static TMP_Text answer;
	public static bool canOpenDoor = false;
	public static GameObject codeLockUI;
	public static GameObject keypad;

	void Start() {
		codeLockUI = GameObject.Find("CodeLockUI").GetComponentInChildren<Canvas>().gameObject;
		keypad = codeLockUI.transform.GetChild(0).gameObject;
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

	public static void CheckCode() {
		if(answer.text.Equals(Door.code)) {
			canOpenDoor = true;
			answer.text = "Correct";
			RemoveKeypadUI();
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
	}

	public static void RemoveKeypadUI() {
		Delete();
		keypad.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		CameraMovement.CanRotate = true;
	}
}
