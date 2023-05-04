using TMPro;
using UnityEngine;

public class Note:MonoBehaviour, IInteractable {
    public float MaxRange { get { return maxRange; } }
    private const float maxRange = 2f;

    private TextMeshProUGUI itemText;
    private GameObject itemUI;

    public void OnEndHover() {
        itemText.text = "";
    }

    public void OnInteract() {

    }

    public void OnStartHover() {
        itemText.text = "Press " + CameraMovement.interactKey + " to read note";
    }

    // Start is called before the first frame update
    void Start() {
        itemUI = GameObject.Find("ItemUI");
        itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
    }
}
