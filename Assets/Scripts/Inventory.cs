using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory:MonoBehaviour {

	[Header("Inventory GameObjects")]
	[SerializeField] private GameObject inventory;
	[SerializeField] private GameObject battery;
	[SerializeField] private GameObject pills;
	[SerializeField] private Spawner spawner;

	[Header("Inventory texts")]
	[SerializeField] private TextMeshProUGUI batteryText;
	[SerializeField] private TextMeshProUGUI drugText;

	[Header("Inventory Sliders")]
	[SerializeField] private Slider flashlightSlider;
	[SerializeField] private Slider cameraSlider;

	[Header("Inventory values")]
	public static int drugNr;
	public static int batteryNr;
	public static bool inventoryOpen = false;
	public static List<int> keyIds = new();

	public static bool canOpenInventory = true;
	private void Start() {
		//Initializes sliders
		flashlightSlider.value = Flashlight.batteryLevel;
		cameraSlider.value = PhotoCapture.charges;
		spawner = GetComponent<Spawner>();
	}

	// Update is called once per frame
	void Update() {
		if(inventoryOpen && !canOpenInventory) {
			inventory.SetActive(false);
			inventoryOpen = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		//Keeps number of pills and batteries in the inventory up to date.
		drugText.text = drugNr.ToString();
		batteryText.text = batteryNr.ToString();

		//Keeps sliders up to date.
		flashlightSlider.value = Flashlight.batteryLevel;
		cameraSlider.value = PhotoCapture.charges;

		//Toggles inventory on and off, this also toggles cameramovement action camera and the cursor.
		if(Input.GetKeyDown(FirstPersonController.openInventory) && !PauseMenu.paused && !PauseMenu.pausedClient && canOpenInventory) {
			switch(inventory.activeSelf) {
				case true:
				inventory.SetActive(false);
				inventoryOpen = false;
				Cursor.lockState = CursorLockMode.Locked;
				CameraMovement.CanRotate = true;
				PhotoCapture.canUseCamera = true;
				break;

				case false:
				inventory.SetActive(true);
				inventoryOpen = true;
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				CameraMovement.CanRotate = false;
				PhotoCapture.canUseCamera = false;
				break;
			}
		}
	}

	/// <summary>
	/// UseDrugs decrements the number of pills seen in the inventory by 1 and removes paranoia from the player.
	/// </summary>
	public void UseDrugs() {
		drugNr = int.Parse(drugText.text);
		if(drugNr > 0) {
			drugNr--;
		}
	}
	/// <summary>
	/// DropDrugs spawns in the prefab "pills" at the players position and decrements the number of pills seen in the inventory by 1.
	/// </summary>
	public void DropDrugs() {
		drugNr = int.Parse(drugText.text);
		if(drugNr > 0) {
			drugNr--;
			spawner.SpawnPillServerRpc(FirstPersonController.characterController.transform.position + new Vector3(0, 1, 0.2f));
		}
	}
	/// <summary>
	/// RechargeCamera decrements the number of batteries seen in inventory by 1 and adds 1 to the camera charges.
	/// </summary>
	public void RechargeCamera() {
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0 && cameraSlider.value < 3) {
			batteryNr--;
			PhotoCapture.charges++;
		}
	}
	/// <summary>
	/// RechargeFlashlight decrements the number of batteries seen in inventory by 1 and adds to the Flashlights batterylevel.
	/// </summary>
	public void RechargeFlashlight() {
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0 && Flashlight.batteryLevel != 100) {
			batteryNr--;
			Flashlight.batteryLevel = Flashlight.batteryLevel >= 80 ? 100 : Flashlight.batteryLevel += 20;
		}
	}
	//DropBattery spawns in the prefab "battery" at the players position and decrements the number of batteries seen in the inventory by 1.
	public void DropBattery() {
		batteryNr = int.Parse(batteryText.text);
		if(batteryNr > 0) {
			batteryNr--;
			spawner.SpawnBatteryServerRpc(FirstPersonController.characterController.transform.position + new Vector3(0, 1, 0.2f));
		}
	}
}
