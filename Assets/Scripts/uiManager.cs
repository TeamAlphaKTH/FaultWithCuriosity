using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager:NetworkBehaviour {
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			SceneManager.LoadScene(0);
		}
	}
}
