using TMPro;
using UnityEngine;

public class Door:MonoBehaviour, IInteractable {
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;
	private bool isOpen = false;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(!animator.GetBool("OpenDoor")) {
			animator.SetBool("OpenDoor", true);
			animator.SetBool("CloseDoor", false);
		} else {
			animator.SetBool("OpenDoor", false);
			animator.SetBool("CloseDoor", true);
		}
	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to use door";
	}
	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();
	}
}
