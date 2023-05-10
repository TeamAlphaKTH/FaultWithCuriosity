using TMPro;
using UnityEngine;

public class InputScript:MonoBehaviour {
	[SerializeField] private TMP_InputField input;

	[Header("Portnumber")]
	[SerializeField] public int portNumber;

	// Start is called before the first frame update
	void Start() {
		portNumber = 7777;
		input.onValueChanged.AddListener(UpdatePortNumber);
	}

	private void UpdatePortNumber(string value) {
		int newPortNumber;
		if(int.TryParse(value, out newPortNumber)) {
			if(newPortNumber > 65535) {
				portNumber = 7777;
			} else if(newPortNumber < 49152) {
				portNumber = 7777;
			} else {
				portNumber = newPortNumber;
			}
		}
	}
}
