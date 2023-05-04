using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Door:NetworkBehaviour, IInteractable {
	[Header("KeyLock")]
	[SerializeField] private bool keyLockDoor = false;
	[SerializeField] private int keyId;

	[Header("CodeLock")]
	[SerializeField] public bool codeLockDoor = false;

	public NetworkVariable<int> code = new(1);

	public static TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;


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
		} else if(codeLockDoor)
			return;

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
			itemText.text = "Door is locked";
		} else {
			itemText.text = "Press " + CameraMovement.interactKey + " to use door";
		}
	}


	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();
		code.Value = int.Parse(Random.Range(0, 10000).ToString("D4"));

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
	[ServerRpc(RequireOwnership = false)]
	public void SetBoolServerRpc() {
		codeLockDoor = false;
		SetBoolClientRpc();
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
	[ClientRpc]
	public void SetBoolClientRpc() {
		codeLockDoor = false;
	}

}
