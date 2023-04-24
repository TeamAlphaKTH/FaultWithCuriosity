using UnityEngine;
using UnityEngine.UI;

public class Flashlight:MonoBehaviour {
    [Header("Flashlight Parameters")]
    [SerializeField] Light FlashlightLight;
    [SerializeField] private Slider batterySlider;
    private bool flashlightActive = false;
    private bool canUseFlashlight = true;
    public float batteryLevel = 100;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;
    private float minIntensity = 1f;
    private float maxIntensity = 7f;
    private float flickerDuration = 0.2f;
    private float flickerDelay = 0.1f;
    private bool isFlickering = false;

    private void Awake() {
        batterySlider.value = 100;
    }

    void Start() {
        // Flashlight starts off
        FlashlightLight.gameObject.SetActive(false);
    }

    void Update() {
        if(canUseFlashlight && batteryLevel != 0) {
            FlashlightControl();
        }
    }

    private void FlashlightControl() {
        // Toggle flashlight on/off with key press
        if(Input.GetKeyDown(flashlightKey)) {
            flashlightActive = !flashlightActive;
            FlashlightLight.gameObject.SetActive(flashlightActive);
        }

        // Handle battery level and flickering
        if(flashlightActive && batteryLevel > 0) {
            batteryLevel -= 10f * Time.deltaTime;
            batterySlider.value = batteryLevel;

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
    }


    private void Flicker() {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        FlashlightLight.intensity = randomIntensity;
    }
}
