using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SinglePlayerBtn:NetworkBehaviour {

	private Button btn;
	private string Ip = "127.0.0.1";
	private ushort port = 7777;

	private void Start() {
		btn = GameObject.Find("/Main Menu/PlayMenu/SinglePlayer Button").GetComponent<Button>();
		btn.onClick.AddListener(() => {
			OnButtonPress();
		});
	}

	public void OnButtonPress() {
		CameraMovement.CanRotate = true;
		FirstPersonController.CanMove = true;
		PhotoCapture.canUseCamera = true;
		Inventory.canOpenInventory = true;
		Inventory.drugNr = 0;
		Inventory.batteryNr = 0;
		Flashlight.batteryLevel = 100;
		PhotoCapture.charges = 3;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		SceneManager.LoadScene("Dungeon");
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
		if(arg0.name.Equals("Dungeon")) {
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(Ip, port, Ip);
			NetworkManager.Singleton.StartHost();
		}
	}
}
