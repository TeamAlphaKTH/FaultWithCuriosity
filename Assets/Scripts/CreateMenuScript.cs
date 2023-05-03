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

    private TMP_InputField portnumberInput;
    private Button btn;
    private bool wrongFormat = false;
    private string localIp;
    private ushort port;

    private void Start() {
        portnumberInput = GameObject.Find("/Main Menu/CreateGameMenu").GetComponentInChildren<TMP_InputField>();
        btn = GameObject.Find("/Main Menu/CreateGameMenu").GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => {
            OnButtonPress();
        });
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    public void OnButtonPress() {
        //find local ips using the [port]
        localIp = GetLocalIpadress();
        port = 0;

        //Parse the port text into a ushort 
        try {
            port = ushort.Parse(portnumberInput.text);
            if(port < 0 || port > 65535) {
                port = 7777;
                portnumberInput.text = localIp + ":" + port;
            }
        } catch(FormatException) {
            Debug.Log("Portnumber cannot contain any letters");
            return;
        }
        portnumberInput.text = localIp + ":" + port;

        //Logic for when port is correct
        if(!wrongFormat) {
            //Load the scene, the EventHandler will start hosting
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