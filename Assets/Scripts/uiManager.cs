using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class uiManager : NetworkBehaviour
{
	private Canvas gameOverUI;
	private Canvas downedScreen;
	private TMP_Text restartText;
	private TMP_Text downedText;
	private bool isGameOver;
	private bool isDowned;

	// Start is called before the first frame update
	void Start()
	{
		gameOverUI = transform.parent.GetChild(0).GetComponent<Canvas>();
		downedScreen = transform.parent.GetChild(1).GetComponent<Canvas>();
		restartText = transform.parent.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
		downedText = transform.parent.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
		gameOverUI.enabled = false;
		downedScreen.enabled = false;
		restartText.gameObject.SetActive(false);
		downedText.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Flashlight.currentParanoia >= 100 && !isDowned)
		{
			FirstPersonController.CanMove = false;
			isDowned = true;
			StartCoroutine(DownedSequence());
		}

		if (Input.GetKeyDown(KeyCode.Q) && isGameOver)
		{
			Application.Quit();
		}
	}

	private IEnumerator GameOverSequence()
	{
		gameOverUI.enabled = true;
		yield return new WaitForSeconds(2.0f);
		restartText.gameObject.SetActive(true);
	}

	private IEnumerator DownedSequence()
	{
		downedScreen.enabled = true;
		downedText.gameObject.SetActive(true);
		int countDown = 30;
		while (countDown > 0)
		{
			downedText.text = "Waiting for revival: " + countDown.ToString();
			yield return new WaitForSeconds(1.0f);
			countDown--;
		}

		if (isDowned)
		{
			isDowned = false;
			downedScreen.enabled = false;
			downedText.gameObject.SetActive(false);
			CameraMovement.CanRotate = false;
			gameOverUI.enabled = true;
			isGameOver = true;
			StartCoroutine(GameOverSequence());
		}
	}
}
