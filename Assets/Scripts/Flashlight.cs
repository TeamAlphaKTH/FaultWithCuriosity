using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight:NetworkBehaviour {
	[Header("Networking")]
	private GameObject cam;
	[SerializeField] public bool isDead;
	private TextMeshProUGUI itemText;
	private GameObject itemUI;
	[SerializeField] private float timer = 30f;
	private float currentTime = 0;

	[Header("Flashlight Parameters")]
	[SerializeField] private Light flashlightLight;
	[SerializeField] private KeyCode flashlightKey = KeyCode.F;
	[SerializeField] private TMP_Text batteryText;
	[SerializeField] private float batterySpeed = 10f;
	[SerializeField] private float incrementBattery = 15f;
	[SerializeField] private KeyCode rechargeBattery = KeyCode.B;
	[SerializeField] private Image batteryBlock1;
	[SerializeField] private Image batteryBlock2;
	[SerializeField] private Image batteryBlock3;

	private bool flashlightActive = true;
	private bool canUseFlashlight = true;
	public static float batteryLevel = 100;
	private float maxIntensity;
	private float flickerDuration = 0.5f;
	private float flickerDelay = 0.1f;
	private bool isFlickering = false;

	[Header("Paranoia parameters")]
	[SerializeField] private float paranoiaIncrements = 1.5f;
	[SerializeField] public static float currentParanoia;
	[SerializeField] private Slider paranoiaSlider;

	public override void OnNetworkSpawn() {
		if(!IsOwner) { return; }

		cam = GetComponentInChildren<Camera>().gameObject;

		flashlightLight = cam.GetComponentInChildren<Light>();


		this.batteryText = GameObject.Find("Battery Percentage").GetComponent<TextMeshProUGUI>();
		this.batteryBlock1 = GameObject.Find("Percentage1").GetComponent<Image>();
		this.batteryBlock2 = GameObject.Find("Percentage2").GetComponent<Image>();
		this.batteryBlock3 = GameObject.Find("Percentage3").GetComponent<Image>();
		this.paranoiaSlider = GameObject.Find("Paranoia Slider").GetComponent<Slider>();

		this.batteryText.SetText("100%");
		currentParanoia = 0;
		this.maxIntensity = flashlightLight.intensity;


		this.paranoiaSlider.value = 0f;
	}

	void Start() {
		if(!IsOwner) { return; }
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
	}

	void Update() {
		if(!IsOwner) { return; }

		if(canUseFlashlight) {
			FlashlightControl();
		}
		if(isDead) {
			CameraMovement.CanRotate = false;
			FirstPersonController.CanMove = false;
			PhotoCapture.canUseCamera = false;
			Inventory.canOpenInventory = false;
			itemText.text = "You are DEAD!!";
			currentTime += Time.deltaTime;
			if(currentTime >= timer) {
				GameOverServerRpc();
			}
		} else { currentTime = 0; }
	}

	private void FlashlightControl() {
		// Toggle flashlight on/off with key press
		if(PauseMenu.paused) {
			return;
		}
		if(Input.GetKeyDown(flashlightKey) && !PauseMenu.pausedClient && batteryLevel > 0) {
			if(flashlightActive) {
				ChangeLightIntensityServerRpc(0);
				flashlightActive = false;
			} else {
				ChangeLightIntensityServerRpc(maxIntensity);
				flashlightActive = true;
			}
		}

		if(!isFlickering && batteryLevel > 10) {
			CancelInvoke("Flicker");
			ChangeLightIntensityServerRpc(maxIntensity);
		}

		if(batteryLevel <= 100 && batteryLevel > 50) {
			batteryBlock3.enabled = true;
			batteryBlock2.enabled = true;
			batteryBlock1.enabled = true;
			batteryBlock3.color = new Color32(0, 255, 19, 120);
			batteryBlock2.color = new Color32(0, 255, 19, 120);
			batteryBlock1.color = new Color32(0, 255, 19, 120);
		} else if(batteryLevel <= 50 && batteryLevel >= 10) {
			batteryBlock3.enabled = false;
			batteryBlock2.enabled = true;
			batteryBlock1.enabled = true;
			batteryBlock2.color = new Color32(255, 241, 0, 120);
			batteryBlock1.color = new Color32(255, 241, 0, 120);
		} else if(batteryLevel < 10 && batteryLevel > 0) {
			batteryBlock2.enabled = false;
			batteryBlock1.enabled = true;
			batteryBlock1.color = new Color32(255, 32, 0, 120);
		} else if(batteryLevel <= 0) {
			batteryBlock1.enabled = false;
		}

		// Handle battery level and flickering
		if(flashlightActive && batteryLevel >= 0) {
			batteryLevel -= batterySpeed * Time.deltaTime;

			if(batteryLevel < 15 && batteryLevel > 0) {
				if(!isFlickering) {
					isFlickering = true;
					InvokeRepeating("Flicker", flickerDelay, flickerDuration);
				}
			} else {
				if(isFlickering) {
					isFlickering = false;
					CancelInvoke("Flicker");
				}
			}
		} else {
			batteryLevel = Mathf.Max(batteryLevel, 0);
			isFlickering = false;
			ChangeLightIntensityServerRpc(0);
			flashlightActive = false;
		}

		if(!flashlightActive && currentParanoia < 100) {
			currentParanoia += paranoiaIncrements * Time.deltaTime;
		}

		if(currentParanoia >= 100) {
			SetDeadServerRpc(true);
		}

		batteryText.SetText(batteryLevel.ToString("F0"));
		paranoiaSlider.value = currentParanoia;
	}

	private void Flicker() {
		float randomIntensity = Random.Range(maxIntensity * 0.1f, maxIntensity * 1.1f);
		ChangeLightIntensityServerRpc(randomIntensity);
	}
	public void Heal() {
		currentParanoia = 60;
	}

	[ClientRpc]
	private void ChangeIntensityClientRpc(float newIntensity) {
		flashlightLight.intensity = newIntensity;
	}
	[ServerRpc]
	public void ChangeLightIntensityServerRpc(float newIntensity) {
		ChangeIntensityClientRpc(newIntensity);
	}
	[ServerRpc(RequireOwnership = false)]
	public void SetDeadServerRpc(bool state) {
		SetDeadClientRpc(state);
	}
	[ClientRpc]
	public void SetDeadClientRpc(bool state) {
		isDead = state;
	}
	[ServerRpc(RequireOwnership = false)]
	public void GameOverServerRpc() {
		GameOverClientRpc();
	}
	[ClientRpc]
	public void GameOverClientRpc() {
		NetworkManager.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
}
