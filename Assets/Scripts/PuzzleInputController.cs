using System.Collections.Generic;
using UnityEngine;

public class PuzzleInputController:MonoBehaviour {
	[SerializeField] private List<GameObject> buttons = new();
	[SerializeField] private GameObject door;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		foreach(GameObject button in buttons) {
			if(!button.GetComponentInChildren<ButtonController>().clicked) {
				return;
			}
		}

		door.GetComponent<Door>().locked = false;
	}
}
