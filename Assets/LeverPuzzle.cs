using UnityEngine;

public class LeverPuzzle:MonoBehaviour {

    private static string leverTag;

    [SerializeField] private bool lever = false;
    private static int inputLever;
    [SerializeField] static int count = 0;
    bool correctCode = false;
    bool unlockDoor = false;
    [SerializeField] private GameObject door;


    //int[] correctOrder = new int[4] { 3, 2, 1, 4 };
    string correctOrder = "3214";
    [SerializeField] public int[] currentInputOrder = new int[4];
    string intArrayToString;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //lever = Physics.Raycast(transform.position, transform.forward, out hitObject, 6f);

    }

    //Adds the number of the lever pressed which is then checked against the preset code array.
    void leverPress(int input) {
        if(count < 4) {
            currentInputOrder[count] = input;
            count++;
        } else if(count == 4) {
            Debug.Log("Not allowed more than 4");
        }

    }

    /*public static void leverInput() {
        if(Input.GetKeyDown(CameraMovement.interactKey) && lever) {
            leverTag = hitObject.collider.gameObject.tag;
            if(leverTag == "Lever1") {
                inputLever = 1;
                leverPress(inputLever);
            } else if(leverTag == "Lever2") {
                inputLever = 2;
                leverPress(inputLever);
            } else if(leverTag == "Lever3") {
                inputLever = 3;
                leverPress(inputLever);
            } else if(leverTag == "Lever4") {
                inputLever = 4;
                leverPress(inputLever);
            } else if(leverTag == "ResetButton") {
                clearArray();
            }

        }
        if(count == 4) {
            intArrayToString = string.Join("", currentInputOrder);
            correctCode = correctOrder.Equals(intArrayToString);
            if(correctCode) {
                unlockDoor = true;
            } else {
                unlockDoor = false;
                clearArray();
            }
        }
    }
    //Clears the array which checks the inputed order.
    void clearArray() {
        for(int i = 0; i < correctOrder.Length; i++) {
            currentInputOrder[i] = 0;
        }
        count = 0;
    }*/

}
