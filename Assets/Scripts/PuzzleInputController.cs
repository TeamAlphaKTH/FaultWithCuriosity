using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
internal class Inputs {
	public GameObject inputObject;
	public bool enabled;
}

public class PuzzleInputController:NetworkBehaviour {
	[SerializeField] private List<Inputs> inputs = new();
	[SerializeField] private GameObject door;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		foreach(Inputs input in inputs) {
			//Get if the input is currently clicked/enabled
			bool clicked = input.inputObject.GetComponentInChildren<ButtonController>().clicked;
			//Get if that input object should be clicked/enabled
			bool enabled = input.enabled;
			//Stop looping through if one is false
			if((!clicked && enabled) || (clicked && !enabled)) {
				return;
			}
		}

		//Unlock the defined door if everything is correct
		UnlockDoorServerRpc();
	}

	[ServerRpc(RequireOwnership = false)]
	private void UnlockDoorServerRpc() {
		UnlockDoorClientRpc();
	}

	[ClientRpc]
	private void UnlockDoorClientRpc() {
		door.GetComponent<Door>().locked = false;
	}
}
