using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight:MonoBehaviour {
    [Header("Flashlight Parameters")]
    [SerializeField] Light FlashlightLight;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private float batterySpeed = 10f;
    [SerializeField] private float incrementBattery = 15f;
    [SerializeField] private KeyCode rechargeBattery = KeyCode.B;
    [SerializeField] private Image batteryBlock1;
    [SerializeField] private Image batteryBlock2;
    [SerializeField] private Image batteryBlock3;

    private bool flashlightActive = false;
    private bool canUseFlashlight = true;
    public static float batteryLevel = 100f;
    private float minIntensity = 1f;
    private float maxIntensity = 7f;
    private float flickerDuration = 0.65f;
    private float flickerDelay = 0.1f;
    [SerializeField] private bool isFlickering = false;

    [Header("Paranoia parameters")]
    [SerializeField] private float maxParanoia = 100f;
    [SerializeField] private float paranoiaIncrements = 0.2f;
    [SerializeField] private float currentParanoia;
    [SerializeField] private Slider paranoiaSlider;

    private void Awake() {
        batteryText.SetText("100%");
        currentParanoia = 0;
    }

    void Start() {
        // Flashlight starts off
        FlashlightLight.gameObject.SetActive(false);
        paranoiaSlider.value = 0f;
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

            if(!isFlickering && batteryLevel > 33) {
                CancelInvoke("Flicker");
                FlashlightLight.intensity = maxIntensity;
            }
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

            if(batteryLevel < 10) {
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

        if(!flashlightActive && currentParanoia < 100) {
            currentParanoia = currentParanoia * 1.001f + paranoiaIncrements * Time.deltaTime;
        }

        if(currentParanoia >= 100) {
            Debug.Log("Dead");
        }

        batteryText.SetText(batteryLevel.ToString("F0"));
        paranoiaSlider.value = currentParanoia;
    }

    private void Flicker() {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        FlashlightLight.intensity = randomIntensity;
    }
}
