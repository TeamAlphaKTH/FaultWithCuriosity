using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript:NetworkBehaviour {
	[SerializeField] private Button hostButton;
	[SerializeField] private Button connectButton;

	private void Awake() {
		hostButton.onClick.AddListener(() => {
			NetworkManager.Singleton.StartHost();
		});
		connectButton.onClick.AddListener(() => {
			NetworkManager.Singleton.StartClient();
		});
	}

	private void Update() {

	}
}
