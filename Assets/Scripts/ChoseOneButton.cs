using Unity.Netcode;
using UnityEngine;

public class ChoseOneButton:NetworkBehaviour {
	[SerializeField] private PuzzleInputController controller;

	[SerializeField] public NetworkVariable<int> randomNumber = new(0);
	private int numOfInputs;
	// Start is called before the first frame update
	public override void OnNetworkSpawn() {
		numOfInputs = controller.inputs.Count;
		if(IsHost) {
			randomNumber.Value = UnityEngine.Random.Range(1, numOfInputs + 1);
		}
		int counter = 0;
		foreach(var input in controller.inputs) {
			if(counter < randomNumber.Value) {
				counter++;
				continue;
			}
			input.enabled = true;
			break;
		}
		base.OnNetworkSpawn();
	}
}
