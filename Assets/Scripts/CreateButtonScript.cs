using System;
using System.Net;
using System.Net.Sockets;

public class JoinButtonScript {
	public void OnButtonPress() {
		//find local ips using the [port]
		string localIp = GetLocalIpadress();
		//NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, port);

		//Needs to have Netcode downloaded
		//start hosting, show text during COOP option and in pasue menu
		//NetworkManager.Singleton.StartHost();
	}

	public static string GetLocalIpadress() {
		IPHostEntry ips = Dns.GetHostEntry(Dns.GetHostName());
		foreach(var ip in ips.AddressList) {
			if(ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().EndsWith("1"))
				return ip.ToString();
		}
		throw new Exception("Could not find current (if any) connected IPv4 address");
	}
}
