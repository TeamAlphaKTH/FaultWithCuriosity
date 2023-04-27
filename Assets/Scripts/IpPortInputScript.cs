using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IpPortInputScript : MonoBehaviour {
	[SerializeField] private TMP_InputField ipAndPort;
	[SerializeField] private Button joinButton;
	string inputText;

	public int portNumber;
	public string ipAddress;

	void Start() {
		joinButton.onClick.AddListener(OnSubmitInfo);
	}

	private void OnSubmitInfo() {
		inputText = ipAndPort.text;
		string[] stringParts = inputText.Split(":");
		if (stringParts.Length == 2) {
			ipAddress = stringParts[0].Trim();
			int.TryParse(stringParts[1].Trim(), out portNumber);
			if (portNumber > 65535) {
				portNumber = 65535;
			} else if (portNumber < 49152) {
				portNumber = 49152;
			}
			//NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, port);
			//NetworkManager.Singleton.StartClient();
		}
	}
}
