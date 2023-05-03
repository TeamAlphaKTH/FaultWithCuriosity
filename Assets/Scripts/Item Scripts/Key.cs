using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Key:NetworkBehaviour, IInteractable {
	[SerializeField] private int id = 0;

	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;

	// Start is called before the first frame update
	void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
	}
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		Inventory.keyIds.Add(id);
		NetworkObjectReference key = gameObject;
		RemoveKeyServerRpc(key);
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to pick up key";
	}
	[ServerRpc(RequireOwnership = false)]
	private void RemoveKeyServerRpc(NetworkObjectReference key) {
		NetworkObject key1 = key;
		key1.Despawn();
	}
}
