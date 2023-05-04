using TMPro;
using UnityEngine;

public class Lever:MonoBehaviour, IInteractable {
    private TextMeshProUGUI itemText;
    private GameObject itemUI;

    private bool lever1 = false;
    private bool lever2 = false;
    private bool lever3 = false;
    private bool lever4 = false;
    [SerializeField] public static int binaryCode = 0;
    public float MaxRange { get { return maxRange; } }
    private const float maxRange = 100f;
    public void OnEndHover() {
        itemText.text = "";
    }

    public void OnInteract() {
        if(gameObject.CompareTag("Lever1")) {
            lever1 = !lever1;
            if(lever1) {
                binaryCode += 8;
            } else {
                binaryCode -= 8;
            }
        }
        if(gameObject.CompareTag("Lever2")) {
            lever2 = !lever2;
            if(lever2) {
                binaryCode += 4;
            } else {
                binaryCode -= 4;
            }
        }
        if(gameObject.CompareTag("Lever3")) {
            lever3 = !lever3;
            if(lever3) {
                binaryCode += 2;
            } else {
                binaryCode -= 2;
            }
        }
        if(gameObject.CompareTag("Lever4")) {
            lever4 = !lever4;
            if(lever4) {
                binaryCode += 1;
            } else {
                binaryCode -= 1;
            }
        }
    }

    public void OnStartHover() {
        itemText.text = "Press " + CameraMovement.interactKey + " to activate lever";
    }

    private void Start() {
        itemUI = GameObject.Find("ItemUI");
        itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
    }
}
