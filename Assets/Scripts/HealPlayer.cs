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
		if(flashlight.isDead && Inventory.drugNr > 0) {
			flashlight.SetDeadServerRpc(false);
			flashlight.HealServerRpc();
			Inventory.drugNr--;
			flashlight.hasPlayed = false;
			OnEndHover();
		}

	}

	public void OnStartHover() {
		if(flashlight.isDead && Inventory.drugNr > 0) {
			itemText.text = "Press " + CameraMovement.interactKey + " to revive";
		} else if(flashlight.isDead) {
			itemText.text = "Get pills to revive";
		} else {
			itemText.text = "";
		}

	}

	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		flashlight = GetComponent<Flashlight>();
	}
}
