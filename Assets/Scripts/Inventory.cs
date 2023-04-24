using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory:MonoBehaviour {
    [SerializeField] private KeyCode openInventory = KeyCode.Tab;
    [SerializeField] private GameObject inventory;
    [SerializeField] private TextMeshProUGUI drugText;
    private int drugNr;
    [SerializeField] private GameObject battery;

    [SerializeField] private TextMeshProUGUI batteryText;
    private int batteryNr;
    [SerializeField] private Slider flashlightSlider;
    [SerializeField] private Slider cameraSlider;

    private void Start() {
        flashlightSlider.value = Flashlight.batteryLevel;
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

        flashlightSlider.value = Flashlight.batteryLevel;
    }

    public void UseDrugs() {
        drugNr = int.Parse(drugText.text);
        if(drugNr > 0) {
            drugNr--;
            drugText.text = drugNr.ToString();
        }
    }

    public void DropDrugs() {
        drugNr = int.Parse(drugText.text);
        if(drugNr > 0) {
            drugNr--;
            drugText.text = drugNr.ToString();
        }
    }

    public void RechargeCamera() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0 && cameraSlider.value < 3) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
            cameraSlider.value++;
        }

    }
    public void RechargeFlashlight() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0 && Flashlight.batteryLevel != 100) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
            Flashlight.batteryLevel = Flashlight.batteryLevel >= 80 ? 100 : Flashlight.batteryLevel += 20;
        }

    }
    public void DropBattery() {
        batteryNr = int.Parse(batteryText.text);
        if(batteryNr > 0) {
            batteryNr--;
            batteryText.text = batteryNr.ToString();
            Instantiate(battery, FirstPersonController.characterController.transform.position + new Vector3(0, 1, 0.2f), Quaternion.identity);

        }
    }

}
