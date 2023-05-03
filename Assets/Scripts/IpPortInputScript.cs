using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IpPortInputScript : NetworkBehaviour {
	[SerializeField] private TMP_InputField ipAndPort;
	[SerializeField] private Button joinButton;
	string inputText;

	public ushort portNumber;
	public string ipAddress;

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
			if (0 < portNumber || portNumber > 65535) {
				portNumber = 7777;
			}
			SceneManager.LoadScene("Dungeon");
		}
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
		if (arg0.name.Equals("Dungeon")) {
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, portNumber);
			NetworkManager.Singleton.StartClient();
		}
	}
}