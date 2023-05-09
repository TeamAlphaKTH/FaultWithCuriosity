using TMPro;
using UnityEngine;

public class HealPlayer:MonoBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Flashlight flashlight;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(flashlight.isDead) {
			CameraMovement.CanRotate = true;
			FirstPersonController.CanMove = true;
			PhotoCapture.canUseCamera = true;
			Inventory.canOpenInventory = true;
			itemText.text = "";
			flashlight.SetDeadServerRpc(false);
			flashlight.HealClientRpc();
		}
	}

	public void OnStartHover() {
		if(flashlight.isDead) {
			itemText.text = "Press " + CameraMovement.interactKey + " to revive";
		} else {
			itemText.text = "";
			OnStartHover();
		}
	}

	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		flashlight = GetComponent<Flashlight>();
	}
}
