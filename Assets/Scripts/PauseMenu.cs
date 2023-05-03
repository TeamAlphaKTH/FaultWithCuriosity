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
		PauseMenuCanvas.SetActive(false);
		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update() {
		if(paused || pausedClient) {
			Stop();
		} else {
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
		if(OptionsMenu.activeSelf) {
			OptionsMenu.SetActive(false);
		}
	}
	public void MainMenuButton() {
		pausedClient = false;
		paused = false;
		PauseMenuCanvas.SetActive(false);
		if(IsHost) {
			NetworkManager.Singleton.DisconnectClient(OwnerClientId);
			NetworkManager.Singleton.Shutdown();
		} else {
			leaveServerRpc(NetworkManager.Singleton.LocalClientId);
		}
		SceneManager.LoadScene("MainMenu");
	}

	// Sends to all clients 
	[ServerRpc]
	public void pauseServerRpc(bool state) {
		pauseClientRpc(state);
	}

	[ServerRpc(RequireOwnership = false)]
	public void leaveServerRpc(ulong clientId) {
		NetworkManager.ConnectedClients[clientId].PlayerObject.Despawn();
		//leaveClientRpc(clientId);
	}

	// Changes states at client on demand to server 
	[ClientRpc]
	public void pauseClientRpc(bool state) {
		paused = state;
		if(!paused) {
			Time.timeScale = 1f;
		} else {
			Time.timeScale = 0f;
		}
	}

	[ClientRpc]
	public void leaveClientRpc(ulong clientId) {
		if(OwnerClientId == clientId) {
			NetworkManager.Singleton.DisconnectClient(clientId);
			NetworkManager.Singleton.Shutdown();
		}
	}
}
