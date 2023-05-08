using TMPro;
using UnityEngine;

public class healPlayer:MonoBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;

	private Flashlight user;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;

	// Start is called before the first frame update
	void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		user = GetComponent<Flashlight>();
	}
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(user.isDead && Inventory.drugNr > 0) {
			user.HealServerRpc();
			user.IsDeadServerRpc(false);
			Inventory.drugNr--;
		}
	}

	public void OnStartHover() {
		if(user.isDead && Inventory.drugNr > 0) {
			itemText.text = "Press " + CameraMovement.interactKey + " to revive";
		} else if(user.isDead) {
			itemText.text = "Get a pill to revive";
		}
	}

}
