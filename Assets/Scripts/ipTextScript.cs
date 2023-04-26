using TMPro;
using UnityEngine;

public class ipTextScript : MonoBehaviour {
	[SerializeField] private TMP_Text ipText;
	// Start is called before the first frame update
	void Start() {

		string ipAddress = GetIP();
		ipText.text = "Your IP: " + ipAddress;
	}

	private string GetIP() {
		string IpString = CreateButtonScript.GetLocalIpadress();
		return IpString;
	}
}
