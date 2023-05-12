using System;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;


public class RandomizerCode:NetworkBehaviour {

	[Header("Puzzle Controller")]
	[SerializeField] private PuzzleInputController m_InputController;

	[Header("Settings")]
	[SerializeField] public bool useHexa = false;
	[SerializeField] public bool useUniqueRange = true;

	[TextArea]
	public string usage =
		"Put this script on the gameobject named \"Note\" prefab\n\n" +
		"To get the random number, write \"[]\" where you want the number in your note text";

	[SerializeField] public NetworkVariable<int> randomNum = new(0);
	private TMP_Text m_Text;
	private int numOfInputs;
	private int lowestNumber;
	private int highestNumber;

	public override void OnNetworkSpawn() {
		//Count amount of inputs
		numOfInputs = m_InputController.inputs.Count;

		if(IsHost) {
			//Settings
			if(useUniqueRange) {
				lowestNumber = 256;
				highestNumber = 4095;
			} else {
				lowestNumber = 1;
				highestNumber = (int)Math.Pow(2, numOfInputs);
			}

			//Genrate random number based on amounf of levers, Used by TMP_Text component of "Text"
			randomNum.Value = UnityEngine.Random.Range(lowestNumber, highestNumber);
		}
		Setup();
		base.OnNetworkSpawn();
	}

	private void Setup() {
		if(m_InputController == null)
			throw new MissingReferenceException("Component: InputController was not been assigned properly");

		//Assign TMP_Text component
		m_Text = transform.GetChild(1).GetChild(0).GetComponentInChildren<TMP_Text>();

		//generate the custom bool-list
		//starting with least significant at index0 and most significant at the last index
		bool[] inputState = CreateBoolList(numOfInputs, randomNum.Value);

		//Change enable states for Input objects
		int pointer = 0;
		foreach(PuzzleInputController.Inputs input in m_InputController.inputs) {
			input.enabled = inputState[pointer++];
		}

		//UPDATE THE TEXT TO CONTAIN randomNum
		if(!m_Text.name.Equals("Text")) {
			return;
		}
		if(!useHexa) {
			StringBuilder n = new(m_Text.text);
			n = n.Replace("[]", randomNum.Value.ToString());
			m_Text.text = n.ToString();
		} else if(useHexa) {
			StringBuilder n = new(m_Text.text);
			n = n.Replace("[]", "0x" + randomNum.Value.ToString("X"));
			m_Text.text = n.ToString();
		}
	}
	/// <summary>
	/// This function creates the list of states that the inputs should react on.
	/// basically, 0b0101 (5) -> [true, false, true, false] 
	/// (using little endian, first index is the same as 2^0, 2^1, 2^2, 2^3)
	/// </summary>
	/// <param name="levers">amount of levers</param>
	/// <param name="number">number in the range [1..2^levers)</param>
	/// <returns>A bool-list for each bit represented as a bollean true/false</returns>
	private bool[] CreateBoolList(int levers, int number) {

		//number of inputs states
		bool[] bools = new bool[levers];
		int pointer = 0;

		//Convert the number to bytes, essentially through 
		byte[] bytes = BitConverter.GetBytes(number);

		//Read through each byte and shift each byte eights times
		//between each shift, determine 0 or 1 by AND with 1 (same as 0001 binary or similar) 
		//Debug.Log(number);
		foreach(byte b in bytes) {
			byte bb = b;
			//Out of bounds exception here since the bools array isnt as large as 8 all the times
			for(int i = 0; i < 8 && pointer < levers; i++) {
				bools[pointer++] = (bb & 1) == 1;
				//Debug.Log("2^" + (pointer - 1) + ": " + bools[pointer - 1]);
				bb >>= 1;
			}
		}
		return bools;
	}
}
