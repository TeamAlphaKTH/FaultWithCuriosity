using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture:NetworkBehaviour {
	[Header("Camera Item Parameters")]
	[SerializeField] public static float charges = 3;
	public static bool canUseCamera = true;

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

	// Code needed for finding specific polaroid
	private RaycastHit hitObject;
	private bool itemObject = false;
	private GameObject currentItemPolaroid;
	private GameObject currentPhotoFrame;
	public override void OnNetworkSpawn() {
		if(!IsOwner)
			return;
		GUI = GameObject.Find("ItemUI");
		base.OnNetworkSpawn();
	}
	void Update() {
		if(!IsOwner)
			return;
		if(Input.GetKeyDown(FirstPersonController.useCameraButton) && canUseCamera && !viewingPhoto && charges > 0) {
			cameraFlash.SetActive(true);
			StartCoroutine(CapturePhoto());
			UseCamera();
		}
		// Raycast to see if player is looking at a Polaroid
		itemObject = Physics.Raycast(transform.position, transform.forward, out hitObject, 6f);
		if(!viewingPhoto && Input.GetKeyDown(CameraMovement.interactKey) && itemObject && hitObject.collider.gameObject.CompareTag("Polaroid")) {
			currentItemPolaroid = hitObject.collider.gameObject.transform.parent.gameObject;
			ShowPhoto();
		}

		// Close Photo
		if(viewingPhoto && Input.GetKeyDown(FirstPersonController.useCameraButton)) {
			RemovePhoto();
		}

	}
	/// <summary>
	/// Increases light intensity on object - CameraFlash
	/// </summary>
	/// <returns></returns>
	private IEnumerator CameraFlashEffect() {
		yield return new WaitForSeconds(flashTime);
		cameraFlash.SetActive(false);
	}

	/// <summary>
	/// Captures the photo.
	/// </summary>
	/// <returns></returns>
	private IEnumerator CapturePhoto() {
		// Game UI set to false so that it does not show in screenshot
		GUI.SetActive(false);
		// Wait for end of frame so that the UI is not captured in the screenshot
		yield return new WaitForEndOfFrame();

		// Takes a screenshot of the screen
		Rect regionToRead = new(0, 0, Screen.width, Screen.height);
		Texture2D newTexture = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
		newTexture.ReadPixels(regionToRead, 0, 0, false);
		newTexture.Apply();
		screenCapture = newTexture;

		// Makes a sprite of the screenshot and places it in PhotoDisplayArea in ItemPolaroidObject
		Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
		GameObject photoDisplayAreaObject = itemPolaroid.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
		Image photoDisplayArea = photoDisplayAreaObject.GetComponent<Image>();

		//photoDisplayArea.sprite = photoSprite;

		// Sets PhotoFrameBG (Blank canvas) in ItemPolaroidObject to true
		// the coroutine below needs to be located AFTER the picture is being taken!
		StartCoroutine(CameraFlashEffect());
		GUI.SetActive(true);
		Debug.Log(photoSprite.texture.EncodeToPNG().Length.ToString());
		SpawnItemPolaroid(photoSprite.texture.EncodeToPNG());

		//Scare the enemy away
		EnemyController.ScareTeleport(transform.position);
	}

	/// <summary>
	/// Shows the photo.
	/// </summary>
	public void ShowPhoto() {
		// Set viewingPhoto to true so that player cannot move
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

	/// <summary>
	/// Removes the photo.
	/// </summary>
	private void RemovePhoto() {
		// Set viewingPhoto to false so that player can move
		viewingPhoto = false;
		currentPhotoFrame.SetActive(false);
		NetworkObjectReference polaroid = currentItemPolaroid.GetComponent<NetworkObject>();
		DeletePolaroidServerRpc(polaroid);
		//Destroy(currentItemPolaroid);
		GUI.SetActive(true);
	}

	/// <summary>
	/// Spawns the item polaroid in a random location around infront of the player.
	/// </summary>
	private void SpawnItemPolaroid(byte[] spriteData) {
		// Random position and rotation
		Vector3 randomPosition = new(
			Random.Range(FirstPersonController.characterController.transform.position.x, FirstPersonController.characterController.transform.position.x + 0.5f),
			Random.Range(FirstPersonController.characterController.transform.position.y + 0.5f, FirstPersonController.characterController.transform.position.y + 1.5f),
			Random.Range(FirstPersonController.characterController.transform.position.z, FirstPersonController.characterController.transform.position.z + 0.5f)
			);

		Quaternion randomRotation = Random.rotation;
		// Spawn Polaroid

		SpawnPolaroidServerRpc(randomPosition, randomRotation, spriteData);

	}

	/// <summary>
	/// Method for using camera if the button is pressed and player canUseCamera is true,
	/// charges exceed 0
	/// </summary>
	private void UseCamera() {
		charges--;
	}
	[ServerRpc]
	private void SpawnPolaroidServerRpc(Vector3 pos, Quaternion rot, byte[] spriteBytes) {
		Texture2D texture = new(2, 2);
		texture.LoadImage(spriteBytes);
		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

		GameObject newPolaroid = Instantiate(itemPolaroid, pos, rot);
		newPolaroid.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = sprite;
		newPolaroid.GetComponent<NetworkObject>().Spawn();
	}
	[ServerRpc]
	private void DeletePolaroidServerRpc(NetworkObjectReference polaroid) {
		NetworkObject polaroid1 = polaroid;
		polaroid1.Despawn();
	}
}
