using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Door:NetworkBehaviour, IInteractable {
	[SerializeField] private bool keyLockDoor = false;
	[SerializeField] private int keyId;
	[SerializeField] private bool codeLockDoor = false;
	[SerializeField] public bool canOpenDoor = false;

	public static TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;

	public static string code;

	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;

	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(keyLockDoor) {
			if(Inventory.keyIds.Contains(keyId)) {
				UnlockDoorServerRpc();
				OnStartHover();
			}
			return;
		} else if(codeLockDoor) {
			Keypad.UseKeypad();
			if(canOpenDoor) {
				Keypad.RemoveKeypadUI();
				codeLockDoor = false;
				OnStartHover();
				OpenDoorServerRpc(true);
				CloseDoorServerRpc(false);
			}
			return;
		}

		if(!animator.GetBool("OpenDoor")) {
			OpenDoorServerRpc(true);
			CloseDoorServerRpc(false);
		} else {
			OpenDoorServerRpc(false);
			CloseDoorServerRpc(true);
		}
	}

	public void OnStartHover() {
		if(keyLockDoor && Inventory.keyIds.Contains(keyId)) {
			itemText.text = "Press " + CameraMovement.interactKey + " to unlock the door";
		} else if(codeLockDoor) {
			itemText.text = "Press " + CameraMovement.interactKey + " to enter the code";
		} else if(CompareTag("Key")) {
			itemText.text = "Press " + CameraMovement.interactKey + " to pick up key";
		} else {
			itemText.text = "Press " + CameraMovement.interactKey + " to open door";
		}
	}


	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();
		code = GenerateCode();
		Debug.Log(code);
	}

	private string GenerateCode() {
		int randomNumber = Random.Range(0, 10000);
		string code = randomNumber.ToString("D4");
		return code;
	}
	[ServerRpc(RequireOwnership = false)]
	private void OpenDoorServerRpc(bool state) {
		OpenDoorClientRpc(state);
	}
	[ServerRpc(RequireOwnership = false)]
	private void CloseDoorServerRpc(bool state) {
		CloseDoorClientRpc(state);
	}
	[ServerRpc(RequireOwnership = false)]
	private void UnlockDoorServerRpc() {
		UnlockDoorClientRpc();
	}
	[ClientRpc]
	private void OpenDoorClientRpc(bool state) {
		animator.SetBool("OpenDoor", state);
	}
	[ClientRpc]
	private void CloseDoorClientRpc(bool state) {
		animator.SetBool("CloseDoor", state);
	}
	[ClientRpc]
	private void UnlockDoorClientRpc() {
		keyLockDoor = false;
	}
}
