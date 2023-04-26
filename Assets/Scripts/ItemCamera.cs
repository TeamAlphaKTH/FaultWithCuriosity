using UnityEngine;

public class ItemCamera:MonoBehaviour {
	[Header("Camera Item Parameters")]
	[SerializeField] private int charges = 3;
	[SerializeField] public static KeyCode useCameraButton = KeyCode.Mouse1;
	[SerializeField] private KeyCode rechargeCameraButton = KeyCode.Mouse2;
	[SerializeField] private bool canRechargeCamera;
	public static bool canUseCamera = true;

	// Update is called once per frame
	void Update() {
		canUseCamera = charges > 0 ? true : false;
		if(Input.GetKeyDown(useCameraButton) && canUseCamera && !PhotoCapture.viewingPhoto) {
			UseCamera();
		}
		canRechargeCamera = charges >= 3 ? false : true;
		// Requires to have a battery in the future and its use on camera
		if(Input.GetKeyDown(rechargeCameraButton) && canRechargeCamera) {
			RechargeCamera();
		}
	}

	/// <summary>
	/// Method for using camera if the button is pressed and player canUseCamera is true,
	/// charges exceed 0
	/// </summary>
	private void UseCamera() {
		charges--;
	}

	/// <summary>
	/// Method for recharging camera by testing if there is a battery and it is used on the camera
	/// also that the charges does not exceed 3
	/// </summary>
	private void RechargeCamera() {
		charges++;
	}
}
