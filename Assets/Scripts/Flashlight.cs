using UnityEngine;

public class Flashlight:MonoBehaviour {
    [SerializeField] GameObject FlashlightLight;
    private bool flashlightActive = false;
    private bool canUseFlashlight = true;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;

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
        if(Input.GetKeyDown(flashlightKey)) {
            if(!flashlightActive) {
                FlashlightLight.gameObject.SetActive(true);
                flashlightActive = true;
            } else {
                FlashlightLight.gameObject.SetActive(false);
                flashlightActive = false;
            }
        }
    }
}
