using System;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateButtonScript : NetworkBehaviour {

	[SerializeField] private TMP_InputField portnumberInput;
	[SerializeField] private Button btn;
	public bool wrongFormat = false;

	private void Start() {
		btn.onClick.AddListener(() => {
			OnButtonPress();
		});
	}
	public void OnButtonPress() {
		//find local ips using the [port]
		string localIp = GetLocalIpadress();
		ushort port = 0;
		try {
			port = ushort.Parse(portnumberInput.text);
		} catch (FormatException) {
			Debug.Log("Portnumber cannot contain any letters");
			wrongFormat = true;
		}
		portnumberInput.text = localIp + ":" + port;
		Debug.Log("testing");

		if (!wrongFormat) {
			SceneManager.LoadScene("Dungeon");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, port, localIp);
			NetworkManager.Singleton.StartHost();
			wrongFormat = false;
		}


	}
	public static string GetLocalIpadress() {
		IPHostEntry ips = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in ips.AddressList) {
			if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().EndsWith("1"))
				return ip.ToString();
		}
		throw new Exception("Could not find current (if any) connected IPv4 address");
	}
}
