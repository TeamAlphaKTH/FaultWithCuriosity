using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager:NetworkBehaviour {
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.Q)) {
			if(GameObject.Find("NetworkManager") != null) {
				NetworkManager.Singleton.Shutdown();
				Destroy(GameObject.Find("NetworkManager"));
			}
			SceneManager.LoadScene(0);
		}
	}
}
