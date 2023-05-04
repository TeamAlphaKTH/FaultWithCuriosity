using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetButtons:MonoBehaviour {
	private Button[] buttons;
	private keypadScript keypadScript1;
	private TMP_Text answer;
	private Door door;
	// Start is called before the first frame update
	void Awake() {
		keypadScript1 = transform.parent.parent.parent.GetChild(0).GetComponentInChildren<keypadScript>();
		door = keypadScript1.door;
		buttons = GetComponentsInChildren<Button>();
		answer = transform.parent.GetChild(0).GetComponentInChildren<TMP_Text>();
		buttons[0].onClick.AddListener(() => InputNumber(0));
		buttons[1].onClick.AddListener(() => InputNumber(1));
		buttons[2].onClick.AddListener(() => InputNumber(2));
		buttons[3].onClick.AddListener(() => InputNumber(3));
		buttons[4].onClick.AddListener(() => InputNumber(4));
		buttons[5].onClick.AddListener(() => InputNumber(5));
		buttons[6].onClick.AddListener(() => InputNumber(6));
		buttons[7].onClick.AddListener(() => InputNumber(7));
		buttons[8].onClick.AddListener(() => InputNumber(8));
		buttons[9].onClick.AddListener(() => InputNumber(9));
		buttons[10].onClick.AddListener(() => Enter());
		buttons[11].onClick.AddListener(() => Delete());
	}
	public void Enter() {
		if(answer.text.Equals(door.code.Value.ToString())) {
			answer.text = "";
			door.SetBoolServerRpc();
			keypadScript1.RemoveKeypadUI();
		} else {
			answer.text = "Incorrect";
		}
	}
	public void InputNumber(int number) {
		if(answer.text.Length < 4) {
			answer.text += number.ToString();
		}
	}
	public void Delete() {
		answer.text = "";
	}
}
