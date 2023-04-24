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
    private float flickerDuration = 0.65f;
    private float flickerDelay = 0.1f;
    private bool isFlickering = false;

    [Header("Paranoia parameters")]
    [SerializeField] private float maxParanoia = 100f;
    [SerializeField] private float paranoiaIncrements = 0.2f;
    [SerializeField] private float currentParanoia;
    [SerializeField] private TMP_Text paranoiaText;

    private void Awake() {
        batteryText.SetText("100%");
        paranoiaText.SetText("0%");
        currentParanoia = 0;
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
        } else if(batteryLevel < 0) {
            batteryLevel = Mathf.Max(batteryLevel, 0);
            isFlickering = false;
            FlashlightLight.gameObject.SetActive(false);
            flashlightActive = false;
        } else if(!flashlightActive && currentParanoia < 100) {
            currentParanoia = currentParanoia * 1.001f + paranoiaIncrements * Time.deltaTime;
        }

        if(currentParanoia >= 100) {
            Debug.Log("Dead");
        }

        batteryText.SetText(batteryLevel.ToString("F0") + "%");
        paranoiaText.SetText(currentParanoia.ToString("F0") + "%");
    }

    private void Flicker() {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        FlashlightLight.intensity = randomIntensity;
    }
}
