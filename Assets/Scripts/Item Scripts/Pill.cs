using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Pill:NetworkBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		Inventory.drugNr++;
		NetworkObjectReference drug = gameObject;
		RemoveKeyServerRpc(drug);
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to pick up pills";
	}

	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
	}

	[ServerRpc(RequireOwnership = false)]
	private void RemoveKeyServerRpc(NetworkObjectReference drug) {
		NetworkObject drug2 = drug;
		drug2.Despawn();
	}
}
