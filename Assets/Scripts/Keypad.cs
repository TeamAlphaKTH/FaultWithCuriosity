using UnityEngine;
using UnityEngine.UI;

public class Keypad:MonoBehaviour {
	private Text answer;

	void Start() {
		answer = GetComponent<Text>();
	}

	/// <summary>
	/// Inputs the number.
	/// </summary>
	/// <param name="number">The number.</param>
	public void InputNumber(int number) {
		answer.text += number.ToString();
	}
}
