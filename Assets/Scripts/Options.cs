using TMPro;
using UnityEngine;

public class Options:MonoBehaviour {
	[SerializeField] private string on = "ON";
	[SerializeField] private string off = "OFF";
	[SerializeField] private TextMeshProUGUI headBobText;

	private void Start() {
		headBobText = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
		if(FirstPersonController.canHeadBob) {
			headBobText.text = on;
		} else
			headBobText.text = off;

	}
	public void ToggleHeadBop() {
		if(headBobText.text.Equals(on)) {
			FirstPersonController.canHeadBob = false;
			headBobText.text = off;
		} else {
			FirstPersonController.canHeadBob = true;
			headBobText.text = on;
		}
	}
}
