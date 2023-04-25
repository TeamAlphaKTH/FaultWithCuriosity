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
	private bool viewingPhoto;

	// Start is called before the first frame update
	void Start() {
		screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
	}

	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.Mouse1) && ItemCamera.canUseCamera) {
			if(!viewingPhoto) {
				StartCoroutine(CapturePhoto());
			} else {
				RemovePhoto();
			}
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
		viewingPhoto = true;
		yield return new WaitForEndOfFrame();

		Rect regionToRead = new(0, 0, Screen.width, Screen.height);
		screenCapture.ReadPixels(regionToRead, 0, 0, false);
		screenCapture.Apply();

		StartCoroutine(CameraFlashEffect());
		ShowPhoto();
	}

	public void ShowPhoto() {
		Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
		//photoDisplayArea.sprite = photoSprite;

		photoFrame.SetActive(true);
	}

	private void RemovePhoto() {
		viewingPhoto = false;
		photoFrame.SetActive(false);
	}
}
