using TMPro;
using UnityEngine;

public class Options:MonoBehaviour {
	[SerializeField] private string on = "ON";
	[SerializeField] private string off = "OFF";
	[SerializeField] private TextMeshProUGUI headBobText;

	private void Start() {
		headBobText = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
		if(FirstPersonController.canHeadBop) {
			headBobText.text = on;
		} else
			headBobText.text = off;

	}
	public void ToggleHeadBop() {
		if(headBobText.text.Equals(on)) {
			FirstPersonController.canHeadBop = false;
			headBobText.text = off;
		} else {
			FirstPersonController.canHeadBop = true;
			headBobText.text = on;
		}
	}
}
