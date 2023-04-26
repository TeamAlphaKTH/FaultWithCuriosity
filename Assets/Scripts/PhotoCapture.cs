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
	//[SerializeField] private KeyCode useCamera = KeyCode.Mouse0;
	[SerializeField] private KeyCode closePicture = KeyCode.Mouse1;

	// Raycast
	private RaycastHit hitObject;
	private bool itemObject = false;
	private GameObject currentPhotoFrame;

	// Start is called before the first frame update
	void Start() {
		//screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
	}

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(PlayerActions.useCameraButton) && ItemCamera.canUseCamera && !viewingPhoto) {
			StartCoroutine(CapturePhoto());
		}

		itemObject = Physics.Raycast(transform.position, transform.forward, out hitObject, 6f);
		if(!viewingPhoto && Input.GetKeyDown(PlayerActions.actionButton) && itemObject && hitObject.collider.gameObject.CompareTag("Polaroid")) {
			GameObject currentItemPolaroid = hitObject.collider.gameObject.transform.parent.gameObject;
			ShowPhoto(currentItemPolaroid);
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

	private IEnumerator CapturePhoto() {
		// CameraUI set false
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

	public void ShowPhoto(GameObject currentItemPolaroid) {
		viewingPhoto = true;
		GUI.SetActive(false);
		// Sets PhotoFrameBG (Blank canvas) in ItemPolaroidObject to true
		//Transform position = currentItemPolaroid.transform;
		Debug.Log(currentItemPolaroid.transform.childCount);

		currentPhotoFrame = currentItemPolaroid.transform.GetChild(1).GetChild(0).gameObject;
		currentPhotoFrame.SetActive(true);
		GameObject currentItemPolaroidBody = currentItemPolaroid.transform.GetChild(0).gameObject;
		//photoFrame.SetActive(true);
		Destroy(currentItemPolaroidBody);
	}

	private void RemovePhoto() {
		viewingPhoto = false;
		currentPhotoFrame.SetActive(false);
		Destroy(currentPhotoFrame.transform.parent.parent.gameObject);
		GUI.SetActive(true);
	}

	// Bug - becomes a clone of original object and change with the original
	private void SpawnItemPolaroid() {

		Vector3 randomPosition = new(
			Random.Range(FirstPersonController.characterController.transform.position.x - 0.5f, FirstPersonController.characterController.transform.position.x + 0.5f),
			Random.Range(FirstPersonController.characterController.transform.position.y + 0.5f, FirstPersonController.characterController.transform.position.y + 1.5f),
			Random.Range(FirstPersonController.characterController.transform.position.z - 0.5f, FirstPersonController.characterController.transform.position.z + 0.5f)
			);

		GameObject newPolaroid = Instantiate(itemPolaroid, randomPosition, Quaternion.identity);

	}

}
