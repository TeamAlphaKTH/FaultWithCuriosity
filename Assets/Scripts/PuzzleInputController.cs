using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class Inputs {
	public GameObject inputObject;
	public bool enabled;
}

public class PuzzleInputController:MonoBehaviour {
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
		door.GetComponent<Door>().locked = false;
	}
}
