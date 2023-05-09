using Unity.Netcode;
using UnityEngine;

public class uiManager:NetworkBehaviour {
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			Application.Quit();
		}
	}
}
