using System;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateButtonScript:NetworkBehaviour {

	private TMP_InputField portInput;
	private Button btn;
	private bool wrongFormat = false;
	private string localIp;
	private ushort port;

	private void Start() {
		portInput = GameObject.Find("/Main Menu/CreateGameMenu").GetComponentInChildren<TMP_InputField>();
		btn = GameObject.Find("/Main Menu/CreateGameMenu").GetComponentInChildren<Button>();
		btn.onClick.AddListener(() => {
			OnButtonPress();
		});
	}
	public void OnButtonPress() {
		//Find local ips using the [port]
		localIp = GetLocalIpadress();
		port = 7777;

		//Parse the port text into a ushort 
		if(portInput.text != "") {
			try {
				port = ushort.Parse(portInput.text);
				if(port < 0 || port > 65535) {
					port = 7777;
				}
			} catch(FormatException) {
				Debug.Log("Portnumber format error. Default to 7777");
				return;
			}
		}
		portInput.text = localIp + ":" + port;

		//Logic for when port is correct
		if(!wrongFormat) {
			//Load the scene, the EventHandler will start hosting
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
		wrongFormat = false;
	}
	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
		if(arg0.name.Equals("Dungeon")) {
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, port, localIp);
			NetworkManager.Singleton.StartHost();
		}
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
