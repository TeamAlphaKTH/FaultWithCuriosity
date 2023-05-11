using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IpPortInputScript:NetworkBehaviour {
	private TMP_InputField ipAndPort;
	private Button joinButton;
	string inputText;

	public ushort portNumber;
	public string ipAddress;

	void Start() {
		ipAndPort = GameObject.Find("/Main Menu/Join Game Menu").GetComponentInChildren<TMP_InputField>();
		joinButton = GameObject.Find("/Main Menu/Join Game Menu").GetComponentInChildren<Button>();
		joinButton.onClick.AddListener(OnSubmitInfo);
	}

	private void OnSubmitInfo() {
		inputText = ipAndPort.text;
		string[] stringParts = inputText.Split(":");
		if(stringParts.Length == 2) {
			ipAddress = stringParts[0].Trim();
			ushort.TryParse(stringParts[1].Trim(), out portNumber);
			if(0 < portNumber || portNumber > 65535) {
				portNumber = 7777;
			}
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
			SceneManager.LoadScene("Dungeon");
		}
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
		if(arg0.name.Equals("Dungeon")) {
			CameraMovement.CanRotate = true;
			FirstPersonController.CanMove = true;
			PhotoCapture.canUseCamera = true;
			Inventory.canOpenInventory = true;
			Inventory.drugNr = 0;
			Inventory.batteryNr = 0;
			Flashlight.batteryLevel = 100;
			PhotoCapture.charges = 3;
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, portNumber);
			NetworkManager.Singleton.StartClient();
		}
	}
}
