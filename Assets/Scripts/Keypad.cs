using TMPro;
using UnityEngine;

public class Keypad:MonoBehaviour {
	private TMP_Text answer;

	void Start() {
		answer = GameObject.Find("Input Text").GetComponentInChildren<TMP_Text>();
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
}
