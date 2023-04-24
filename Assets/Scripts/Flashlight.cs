using TMPro;
using UnityEngine;

public class Flashlight:MonoBehaviour {
    [Header("Flashlight Parameters")]
    [SerializeField] Light FlashlightLight;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private float batterySpeed = 10f;
    [SerializeField] private float incrementBattery = 15f;
    [SerializeField] private KeyCode rechargeBattery = KeyCode.B;

    private bool flashlightActive = false;
    private bool canUseFlashlight = true;
    public float batteryLevel = 100;
    private float minIntensity = 1f;
    private float maxIntensity = 7f;
    private float flickerDuration = 0.2f;
    private float flickerDelay = 0.1f;
    private bool isFlickering = false;

    private void Awake() {
        batteryText.SetText("100%");
    }

    void Start() {
        // Flashlight starts off
        FlashlightLight.gameObject.SetActive(false);
    }

    void Update() {
        if(canUseFlashlight) {
            FlashlightControl();
        }
    }

    private void FlashlightControl() {
        // Toggle flashlight on/off with key press
        if(Input.GetKeyDown(flashlightKey)) {
            flashlightActive = !flashlightActive;
            FlashlightLight.gameObject.SetActive(flashlightActive);
        }

        if(Input.GetKeyDown(rechargeBattery)) {
            batteryLevel += incrementBattery;
            if(batteryLevel > 100) {
                batteryLevel = 100;
            }
        }

        // Handle battery level and flickering
        if(flashlightActive && batteryLevel > 0) {
            batteryLevel -= batterySpeed * Time.deltaTime;

            if(batteryLevel < 20) {
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
            FlashlightLight.gameObject.SetActive(false);
            flashlightActive = false;
        }
        batteryText.SetText(batteryLevel.ToString("F0") + "%");
    }


    private void Flicker() {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        FlashlightLight.intensity = randomIntensity;
    }
}
