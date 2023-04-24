using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory:MonoBehaviour {
    [SerializeField] private KeyCode openInventory = KeyCode.Tab;
    [SerializeField] private GameObject inventory = GameObject.Find("Inventory Background");
    [SerializeField] private TextMeshProUGUI drugText;
    private int drugNr;

    [SerializeField] private TextMeshProUGUI batteryText;
    private int batteryNr;
    [SerializeField] private Slider flashlightSlider;
    [SerializeField] private Slider cameraSlider;
    private void Start() {

    }
    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(openInventory)) {
            switch(inventory.activeSelf) {
                case true:
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                FirstPersonController.CanMove = true;
                CameraMovement.CanRotate = true;

                break;
                case false:
                inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                FirstPersonController.CanMove = false;
                CameraMovement.CanRotate = false;
                break;
            }
        }


    }

    public void UseDrugs() {

        drugNr = int.Parse(drugText.text);
        if(drugNr > 0) {
            drugNr--;
            drugText.text = drugNr.ToString();
        }
    }
    public void UseCamera() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
            cameraSlider.value++;
        }

    }
    public void UseFlashlight() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
            flashlightSlider.value++;
        }

    }
    public void DropBattery() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
        }
    }
}
