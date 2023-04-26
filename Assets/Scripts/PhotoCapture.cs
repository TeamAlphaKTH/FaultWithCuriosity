using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture:MonoBehaviour {
	[Header("Photo Taker")]
	[SerializeField] public Image photoDisplayArea;
	[SerializeField] public GameObject photoFrame;

	[Header("Flash Effect")]
	[SerializeField] private GameObject cameraFlash;
	[SerializeField] private float flashTime;

	private Texture2D screenCapture;
	public static bool viewingPhoto = false;

	// All text GUI needs to be in this parent object
	[Header("GUI")]
	[SerializeField] private GameObject GUI;

	[Header("Polaroid GameObject")]
	[SerializeField] private GameObject itemPolaroid;

	[Header("Controls")]
	[SerializeField] private KeyCode closePicture = KeyCode.Mouse1;

	// Raycast
	private RaycastHit hitObject;
	private bool itemObject = false;
	private GameObject currentItemPolaroid;
	private GameObject currentPhotoFrame;

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(ItemCamera.useCameraButton) && ItemCamera.canUseCamera && !viewingPhoto) {
			StartCoroutine(CapturePhoto());
		}

		itemObject = Physics.Raycast(transform.position, transform.forward, out hitObject, 6f);
		if(!viewingPhoto && Input.GetKeyDown(PlayerActions.actionButton) && itemObject && hitObject.collider.gameObject.CompareTag("Polaroid")) {
			currentItemPolaroid = hitObject.collider.gameObject.transform.parent.gameObject;
			ShowPhoto();
		}

		if(viewingPhoto && Input.GetKeyDown(closePicture)) {
			RemovePhoto();
		}
	}
	/// <summary>
	/// Increases light intensity on object - CameraFlash
	/// </summary>
	/// <returns></returns>
	private IEnumerator CameraFlashEffect() {
		cameraFlash.SetActive(true);
		yield return new WaitForSeconds(flashTime);
		cameraFlash.SetActive(false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	private IEnumerator CapturePhoto() {
		// Game UI set to false so that it does not show in screenshot
		GUI.SetActive(false);

		yield return new WaitForEndOfFrame();

		Rect regionToRead = new(0, 0, Screen.width, Screen.height);
		Texture2D newTexture = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
		newTexture.ReadPixels(regionToRead, 0, 0, false);
		newTexture.Apply();
		screenCapture = newTexture;

		// Makes a sprite of the screenshot and places it in PhotoDisplayArea in ItemPolaroidObject
		Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
		photoDisplayArea.sprite = photoSprite;

		StartCoroutine(CameraFlashEffect());
		GUI.SetActive(true);
		SpawnItemPolaroid();
	}

	public void ShowPhoto() {
		viewingPhoto = true;
		GUI.SetActive(false);

		// Object is hardcoded and only The ItemPolaroid within the ItemPolaroid should be Tagged "Polaroid"
		// Sets PhotoFrameBG (Blank canvas) in ItemPolaroidObject to true
		currentPhotoFrame = currentItemPolaroid.transform.GetChild(1).GetChild(0).gameObject;
		currentPhotoFrame.SetActive(true);

		// Destroy the visable body of the ItemPolaroid
		GameObject currentItemPolaroidBody = currentItemPolaroid.transform.GetChild(0).gameObject;
		Destroy(currentItemPolaroidBody);
	}

	private void RemovePhoto() {
		viewingPhoto = false;
		currentPhotoFrame.SetActive(false);
		Destroy(currentItemPolaroid);
		GUI.SetActive(true);
	}

	// Bug - becomes a clone of original object and change with the original
	private void SpawnItemPolaroid() {

		Vector3 randomPosition = new(
			Random.Range(FirstPersonController.characterController.transform.position.x, FirstPersonController.characterController.transform.position.x + 0.5f),
			Random.Range(FirstPersonController.characterController.transform.position.y + 0.5f, FirstPersonController.characterController.transform.position.y + 1.5f),
			Random.Range(FirstPersonController.characterController.transform.position.z, FirstPersonController.characterController.transform.position.z + 0.5f)
			);

		Quaternion randomRotation = Random.rotation;

		GameObject newPolaroid = Instantiate(itemPolaroid, randomPosition, randomRotation);

	}

}
