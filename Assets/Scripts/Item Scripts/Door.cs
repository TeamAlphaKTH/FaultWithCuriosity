using TMPro;
using UnityEngine;

public class Door:MonoBehaviour, IInteractable {
	[SerializeField] private bool locked = false;
	[SerializeField] private int keyId;

	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;

	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(locked) {
			if(Inventory.keyIds.Contains(keyId)) {
				locked = false;
				OnStartHover();
			}
			return;
		}

		if(!animator.GetBool("OpenDoor")) {
			animator.SetBool("OpenDoor", true);
			animator.SetBool("CloseDoor", false);
		} else {
			animator.SetBool("OpenDoor", false);
			animator.SetBool("CloseDoor", true);
		}
	}

	public void OnStartHover() {
		if(locked) {
			if(Inventory.keyIds.Contains(keyId)) {
				itemText.text = "Press " + CameraMovement.interactKey + " to unlock the door";
			} else {
				itemText.text = "The door is locked";
			}
		} else {
			itemText.text = "Press " + CameraMovement.interactKey + " to use door";
		}
	}
	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();
	}
}
