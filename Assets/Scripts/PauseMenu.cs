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
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(paused || pausedClient) {
				Play();
			} else
				Stop();
		}
	}

	// Stops time if host else only pause for client 
	public void Stop() {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
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
	}
	// Plays time and puts pausemenu to false else only unpause client 
	public void Play() {
		Cursor.lockState = CursorLockMode.Confined;
		if(!Inventory.inventoryOpen && !keypadScript.keypadOn) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		PauseMenuCanvas.SetActive(false);
		PausedMenu.SetActive(false);
		if(IsHost) {
			pauseServerRpc(false);
			pausedClient = false;
		} else {
			pausedClient = false;
		}
		if(OptionsMenu.activeSelf) {
			OptionsMenu.SetActive(false);
		}
	}
	public void MainMenuButton() {
		pausedClient = false;
		paused = false;
		PauseMenuCanvas.SetActive(false);
		SceneManager.LoadScene(0);
		if(IsHost) {
			//Host has access to server and therfore wont need to disconnect using a RPC-server request to server
			NetworkManager.Singleton.DisconnectClient(OwnerClientId);
			NetworkManager.Singleton.Shutdown();
		} else {
			var clientIdToDisconnect = NetworkManager.Singleton.LocalClientId;
			leaveServerRpc(clientIdToDisconnect);
			//leaveServerRpc sends to server and as such the 
		}
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		Destroy(NetworkManager.Singleton.gameObject);
	}

	// Sends to Server 
	[ServerRpc]
	public void pauseServerRpc(bool state) {
		pauseClientRpc(state);
	}

	//any client should be able to do this Server RPC
	[ServerRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
	public void leaveServerRpc(ulong clientId) {
		NetworkManager.Singleton.DisconnectClient(clientId);
		leaveClientRpc(clientId);
	}

	// Changes states at client on demand from the server 
	[ClientRpc]
	public void pauseClientRpc(bool state) {
		paused = state;
		if(!paused) {
			Time.timeScale = 1f;
		} else {
			Time.timeScale = 0f;
		}
	}
	//Send to all connected clients
	[ClientRpc(Delivery = RpcDelivery.Reliable)]
	public void leaveClientRpc(ulong clientId) {
		if(NetworkManager.LocalClientId == clientId) {
			NetworkManager.Singleton.Shutdown();
		}
	}
}
