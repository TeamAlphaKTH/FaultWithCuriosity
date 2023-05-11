using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PuzzleInputController:NetworkBehaviour {
	[System.Serializable]
	public class Inputs {
		public GameObject inputObject;
		public bool enabled;
	}

	[SerializeField] public List<Inputs> inputs = new();
	[SerializeField] private GameObject door;

	// Start is called before the first frame update
	void Start() {
		if(!IsOwner)
			return;
	}

	// Update is called once per frame
	private void FixedUpdate() {
		if(!IsOwner)
			return;
		foreach(Inputs input in inputs) {
			bool lockState = door.GetComponent<Door>().locked;

			//Get if the input is currently clicked/enabled
			bool clicked = input.inputObject.GetComponentInChildren<ButtonController>().clicked;
			//Get if that input object should be clicked/enabled
			bool enabled = input.enabled;

			//Check if an input is wrong
			if((!clicked && enabled) || (clicked && !enabled)) {

				//Lock the door if it's not already locked
				if(!lockState)
					ChangeDoorLockStateServerRpc(true);

				//Stop checking for the others
				return;
			}
		}

		//Unlock the defined door if everything is correct
		ChangeDoorLockStateServerRpc(false);
	}

	[ServerRpc(RequireOwnership = false)]
	private void ChangeDoorLockStateServerRpc(bool state) {
		ChangeDoorLockStateClientRpc(state);
	}

	[ClientRpc]
	private void ChangeDoorLockStateClientRpc(bool state) {
		door.GetComponent<Door>().locked = state;
	}
}

