using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits:MonoBehaviour {

	private void Start() {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}
	public void MainMenu() {
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

}
