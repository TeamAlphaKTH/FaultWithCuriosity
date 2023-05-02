using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu:NetworkBehaviour {

	public static bool paused = false;
	public static bool pausedClient = false;
	[SerializeField] private GameObject PauseMenuCanvas;
	// Start is called before the first frame update
	void Start() {
		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update() {
		if(!Inventory.inventoryOpen) {
			if(paused || pausedClient)
				Stop();
			else
				Play();
		}
		if(Input.GetKeyDown(KeyCode.P)) {
			if(paused || pausedClient) {
				Play();
			} else
				Stop();
		}

	}

	public void Stop() {
		PauseMenuCanvas.SetActive(true);
		if(IsHost) {
			Time.timeScale = 0f;
			pauseServerRpc(true);
			pausedClient = true;
		} else {
			pausedClient = true;
			Debug.Log(pausedClient);
		}
		Cursor.lockState = CursorLockMode.Confined;
	}
	public void Play() {
		PauseMenuCanvas.SetActive(false);
		if(IsHost) {
			Time.timeScale = 1f;
			pauseServerRpc(false);
			pausedClient = false;
		} else {
			pausedClient = false;
			Debug.Log(pausedClient);
		}
		if(!Inventory.inventoryOpen) {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	public void MainMenuButton() {
		SceneManager.LoadScene("MainMenu");
	}

	[ServerRpc]
	public void pauseServerRpc(bool state) {
		pauseClientRpc(state);
	}

	[ClientRpc]
	public void pauseClientRpc(bool state) {
		paused = state;
	}
}

