using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IpPortInputScript : NetworkBehaviour {
	[SerializeField] private TMP_InputField ipAndPort;
	[SerializeField] private Button joinButton;

	private string inputText;
	private ushort portNumber;
	private string ipAddress;

	void Start() {
		joinButton.onClick.AddListener(OnSubmitInfo);
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	private void OnSubmitInfo() {
		inputText = ipAndPort.text;
		string[] stringParts = inputText.Split(":");
		if (stringParts.Length == 2) {
			ipAddress = stringParts[0].Trim();
			ushort.TryParse(stringParts[1].Trim(), out portNumber);
			if (portNumber > 65535) {
				portNumber = 7777;
			} else if (portNumber < 0) {
				portNumber = 7777;
			}
			SceneManager.LoadScene("Dungeon");
		} else if (stringParts.Length == 1 && stringParts[0].Contains(".")) {
			ipAddress = stringParts[0].Trim();
			portNumber = 7777;
			SceneManager.LoadScene("Dungeon");
		}
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
		NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, portNumber);
		NetworkManager.Singleton.StartClient();
	}
}
