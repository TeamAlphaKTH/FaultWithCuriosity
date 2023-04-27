using TMPro;
using UnityEngine;

public class InputScript : MonoBehaviour {
	[SerializeField] private TMP_InputField input;

	[Header("Portnumber")]
	[SerializeField] public int portNumber;

	// Start is called before the first frame update
	void Start() {
		portNumber = 49152;
		input.onValueChanged.AddListener(UpdatePortNumber);
	}

	private void UpdatePortNumber(string value) {
		int newPortNumber;
		if (int.TryParse(value, out newPortNumber)) {
			if (newPortNumber > 65535) {
				portNumber = 65535;
			} else if (newPortNumber < 49152) {
				portNumber = 49152;
			} else {
				portNumber = newPortNumber;
			}
		}
	}
}
