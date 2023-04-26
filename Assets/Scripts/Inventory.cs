using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory:MonoBehaviour {
	[SerializeField] private KeyCode openInventory = KeyCode.Tab;
	[SerializeField] private GameObject inventory;
	[SerializeField] private TextMeshProUGUI drugText;
	public static int drugNr;
	[SerializeField] private GameObject battery;
	[SerializeField] private GameObject pills;

	[SerializeField] private TextMeshProUGUI batteryText;
	public static int batteryNr;
	[SerializeField] private Slider flashlightSlider;
	[SerializeField] private Slider cameraSlider;

	private void Start() {
		flashlightSlider.value = Flashlight.batteryLevel;
		Debug.Log("isStart");
	}

	// Update is called once per frame
	void Update() {
		drugText.text = drugNr.ToString();
		batteryText.text = batteryNr.ToString();
		if(Input.GetKeyDown(openInventory) && !PauseMenu.paused) {
			switch(inventory.activeSelf) {
				case true:
				inventory.SetActive(false);
				Cursor.lockState = CursorLockMode.Locked;
				// FirstPersonController.CanMove = true;
				CameraMovement.CanRotate = true;

				break;
				case false:
				inventory.SetActive(true);
				Cursor.lockState = CursorLockMode.Confined;
				// FirstPersonController.CanMove = false;
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
			Flashlight.currentParanoia = Flashlight.currentParanoia <= 20 ? 0 : Flashlight.currentParanoia -= 20;
		}
	}

	public void DropDrugs() {
		drugNr = int.Parse(drugText.text);
		if(drugNr > 0) {
			drugNr--;
			Instantiate(pills, FirstPersonController.characterController.transform.position + new Vector3(0, 1, 0.2f), Quaternion.identity);
		}
	}

	public void RechargeCamera() {
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0 && cameraSlider.value < 3) {
			batteryNr--;
			cameraSlider.value++;
		}

	}
	public void RechargeFlashlight() {
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0 && Flashlight.batteryLevel != 100) {
			batteryNr--;
			Flashlight.batteryLevel = Flashlight.batteryLevel >= 80 ? 100 : Flashlight.batteryLevel += 20;
		}

	}
	public void DropBattery() {
		Debug.Log("isrunning");
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0) {
			batteryNr--;
			Instantiate(battery, FirstPersonController.characterController.transform.position + new Vector3(0, 1, 0.2f), Quaternion.identity);

		}
	}

}
