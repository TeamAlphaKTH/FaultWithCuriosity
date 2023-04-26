using TMPro;
using UnityEngine;

public class Battery:MonoBehaviour, IInteractable {
    private TextMeshProUGUI itemText;
    private GameObject itemUI;
    public float MaxRange { get { return maxRange; } }
    private const float maxRange = 100f;
    public void OnEndHover() {
        itemText.text = "";
    }

    public void OnInteract() {
        Inventory.batteryNr++;
        Destroy(gameObject);
    }

    public void OnStartHover() {
        itemText.text = "Press " + CameraMovement.interactKey + " to pick up battery";
    }
    private void Start() {
        itemUI = GameObject.Find("ItemUI");
        itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
    }
}