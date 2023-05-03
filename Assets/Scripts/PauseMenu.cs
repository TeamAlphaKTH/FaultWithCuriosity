using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu:NetworkBehaviour {

	public static bool paused = false;
	public static bool pausedClient = false;
	[SerializeField] private GameObject PauseMenuCanvas;
	[SerializeField] private GameObject OptionsMenu;
	[SerializeField] private GameObject PausedMenu;
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

	// Stops time if host else only pause for client 
	public void Stop() {
		PauseMenuCanvas.SetActive(true);
		if(OptionsMenu.activeSelf) {
			PausedMenu.SetActive(false);
		} else {
			PausedMenu.SetActive(true);
		}
		if(IsHost) {
			pauseServerRpc(true);
			pausedClient = true;
		} else {
			pausedClient = true;
		}
		Cursor.lockState = CursorLockMode.Confined;
	}
	// Plays time and puts pausemenu to false else only unpause client 
	public void Play() {
		PauseMenuCanvas.SetActive(false);
		PausedMenu.SetActive(false);
		if(IsHost) {
			pauseServerRpc(false);
			pausedClient = false;
		} else {
			pausedClient = false;
		}
		if(!Inventory.inventoryOpen) {
			Cursor.lockState = CursorLockMode.Locked;
		}
		Debug.Log(OptionsMenu.activeSelf);
		if(OptionsMenu.activeSelf) {
			OptionsMenu.SetActive(false);
		}
	}
	public void MainMenuButton() {
		SceneManager.LoadScene("MainMenu");
	}

	// Sends to all clients 
	[ServerRpc]
	public void pauseServerRpc(bool state) {
		pauseClientRpc(state);
	}

	// Changes states at client on demand from server 
	[ClientRpc]
	public void pauseClientRpc(bool state) {
		paused = state;
		if(!paused) {
			Time.timeScale = 1f;
		} else {
			Time.timeScale = 0f;
		}
	}
}
