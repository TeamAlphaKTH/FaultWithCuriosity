using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Battery:NetworkBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		Inventory.batteryNr++;
		NetworkObjectReference battery = gameObject;
		RemoveBatteryServerRpc(battery);
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to pick up battery";
	}

	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
	}

	[ServerRpc(RequireOwnership = false)]
	private void RemoveBatteryServerRpc(NetworkObjectReference battery) {
		NetworkObject battery2 = battery;
		battery2.Despawn();
	}
}
