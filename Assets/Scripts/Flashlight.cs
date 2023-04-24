using UnityEngine;

public class Flashlight:MonoBehaviour {
    [SerializeField] Light FlashlightLight;
    private bool flashlightActive = false;
    private bool canUseFlashlight = true;
    [SerializeField] private float batteryLevel = 100;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;
    private float minIntensity = 1f;
    private float maxIntensity = 7f;
    [SerializeField] private float flickerDuration = 0.2f;
    [SerializeField] private float flickerDelay = 0.1f;
    private bool isFlickering = false;

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
        if(Input.GetKeyDown(flashlightKey)) {
            if(!flashlightActive) {
                FlashlightLight.gameObject.SetActive(true);
                flashlightActive = true;
            } else {
                FlashlightLight.gameObject.SetActive(false);
                flashlightActive = false;
            }
        }

        if(flashlightActive && batteryLevel < 20) {
            if(!isFlickering) {
                isFlickering = true;
                InvokeRepeating("Flicker", flickerDelay, flickerDuration);
            }
        } else if(isFlickering) {
            isFlickering = false;
            CancelInvoke("Flicker");
        }
        Debug.Log("Battery level: " + batteryLevel);
        if(flashlightActive && batteryLevel > 0) {
            batteryLevel -= 10f * Time.deltaTime;
        } else {
            FlashlightLight.gameObject.SetActive(false);
            flashlightActive = false;
        }
    }

    private void Flicker() {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        FlashlightLight.intensity = randomIntensity;
    }
}
