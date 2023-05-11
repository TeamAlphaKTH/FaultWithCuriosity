using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Door:NetworkBehaviour, IInteractable {
	[SerializeField] public bool locked = false;
	[SerializeField] private int keyId;

	[Header("CodeLock")]
	[SerializeField] public bool codeLockDoor = false;

	public NetworkVariable<int> code = new(1);

	public static TextMeshProUGUI itemText;
	private GameObject itemUI;
	private Animator animator;

	[Header("Door Sound")]
	[SerializeField] private AudioSource doorSound;

	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;

	public void OnEndHover() {
		itemText.text = "";
	}

	public void OnInteract() {
		if(locked) {
			if(Inventory.keyIds.Contains(keyId)) {
				UnlockDoorServerRpc();
				OnStartHover();
			}
			return;
		} else if(codeLockDoor)
			return;

		if(!animator.GetBool("OpenDoor")) {
			doorSound.Play();
			OpenDoorServerRpc(true);
			CloseDoorServerRpc(false);
		} else {
			doorSound.Play();
			OpenDoorServerRpc(false);
			CloseDoorServerRpc(true);
		}
	}

	public void OnStartHover() {
		// used for the hidden room in puzzle 6
		if(keyId == 999) {
			if(!locked) {
				itemText.text = "Press " + CameraMovement.interactKey + " to use door";
			}
			return;
		}
		if(locked && Inventory.keyIds.Contains(keyId)) {
			itemText.text = "Press " + CameraMovement.interactKey + " to unlock the door";
		} else if(codeLockDoor || locked) {
			itemText.text = "Door is locked";
		} else {
			itemText.text = "Press " + CameraMovement.interactKey + " to use door";
		}
	}


	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponentInParent<Animator>();

	}
	public override void OnNetworkSpawn() {
		if(IsHost)
			code.Value = int.Parse(Random.Range(1000, 10000).ToString("D4"));
		base.OnNetworkSpawn();
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
		locked = false;
	}
	[ClientRpc]
	public void SetBoolClientRpc() {
		codeLockDoor = false;
	}

}
