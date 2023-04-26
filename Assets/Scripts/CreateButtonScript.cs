using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class CreateButtonScript : MonoBehaviour {

	[SerializeField] private InputField portnumberInput;
	public bool wrongFormat = false;
	public void OnButtonPress() {
		//find local ips using the [port]
		string localIp = GetLocalIpadress();
		ushort port;
		try {
			port = ushort.Parse(portnumberInput.text);
		} catch (FormatException) {
			Debug.Log("Portnumber cannot contain any letters");
			wrongFormat = true;
		}

		if (!wrongFormat) {
			//NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, port);
			//NetworkManager.Singleton.StartHost();
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
