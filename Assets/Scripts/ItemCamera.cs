using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Windows.WebCam;

public class ItemCamera : MonoBehaviour
{
    [Header("Camera Item Parameters")]
    [SerializeField] private int charges = 3;
    [SerializeField] private KeyCode useCameraButton = KeyCode.Mouse1;
    [SerializeField] private KeyCode rechargeCameraButton = KeyCode.Mouse2;
	[SerializeField] private bool canRechargeCamera;
    public static bool canUseCamera = true;
    
	[Header("Polaroid GameObject")]
	[SerializeField] private GameObject itemPolaroid;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
    {
		canUseCamera = charges > 0 ? true : false;
		UseCamera();
		canRechargeCamera = charges >= 3 ? false : true;
		RechargeCamera();
    }

    /// <summary>
    /// Method for using camera if the button is pressed and player canUseCamera is true,
    /// charges exceed 0
    /// </summary>
    private void UseCamera() {
        if(Input.GetKeyDown(useCameraButton)) {
            if(canUseCamera) {
                charges--;
                SpawnItemPolaroid();
            }
        }
    }

    /// <summary>
    /// Method for recharging camera by testing if there is a battery and it is used on the camera
    /// also that the charges does not exceed 3
    /// </summary>
    private void RechargeCamera() {
        if(Input.GetKeyDown(rechargeCameraButton)) {
            // Requires to have a battery in the future and its use on camera
            if(canRechargeCamera) {
                // amount of recharges 
                charges++;
            }
        }
        
    }

	private void SpawnItemPolaroid() {
        Instantiate(itemPolaroid, new Vector3(Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f), Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f), Random.Range(transform.position.z - 0.5f, transform.position.z + 0.5f)), Quaternion.identity);
	}
}
