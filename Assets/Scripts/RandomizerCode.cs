using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;


/// <summary>
/// Put this script on the gameobject named "NoteUI".
/// </summary>
public class RandomizerCode:NetworkBehaviour {

	[Header("Puzzle Controller")]
	[SerializeField] private PuzzleInputController m_InputController;

	private TMP_Text m_Text;
	private GameObject[] levers;
	private int numOfInputs;
	private bool onFlag = true;

	private void Start() {
		if(m_InputController == null)
			throw new MissingReferenceException("Component: InputController was not been assigned properly");

		//Assign components
		m_Text = GameObject.Find("Text").GetComponent<TMP_Text>();

		//Count amount of inputs
		numOfInputs = m_InputController.inputs.Count;

		//Genrate random number based on amounf of levers, Used by TMP_Text component of "Text"
		int randomNum = UnityEngine.Random.Range(1, (int)Math.Pow(2, numOfInputs));

		//generate the custom bool-list
		//starting with least significant at index0 and most significant at the last index
		bool[] inputState = CreateBoolList(numOfInputs, randomNum);

		//Change enable states for Input objects
		int pointer = 0;
		foreach(PuzzleInputController.Inputs input in m_InputController.inputs) {
			input.enabled = inputState[pointer++];
		}

		//UPDATE THE TEXT TO CONTAIN randomNum somwwhere


	}
	//This function creates the list of states that the inputs should react on:
	//true for one
	//false for zero
	private bool[] CreateBoolList(int levers, int number) {

		//number of inputs states
		bool[] bools = new bool[levers];
		int pointer = 0;

		//Convert the number to bytes, essentially through 
		byte[] bytes = BitConverter.GetBytes(number);

		//Read through each byte and shift each byte eights times
		//between each shift, determine 0 or 1 by AND with 1 (same as 0001 binary or similar) 
		Debug.Log(number);
		foreach(byte b in bytes) {
			byte bb = b;
			//Out of bounds exception here since the bools array isnt as large as 8 all the times
			for(int i = 0; i < 8 && pointer < levers; i++) {
				bools[pointer++] = (bb & 1) == 1;
				Debug.Log("2^" + (pointer - 1) + ": " + bools[pointer - 1]);
				bb >>= 1;
			}
		}
		return bools;
	}
}
