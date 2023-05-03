using TMPro;
using UnityEngine;

public class ButtonController:MonoBehaviour, IInteractable {
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 1f;
	private TextMeshProUGUI uiText;
	private Animator animator;

	private bool clicked = false;

	// Start is called before the first frame update
	void Start() {
		uiText = GameObject.Find("ItemUI").GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		if(clicked) {
			animator.SetBool("Clicked", true);
		}
	}

	public void OnEndHover() {
		uiText.text = "";
	}

	public void OnInteract() {
		if(!clicked) {
			clicked = true;
			OnEndHover();
		}
	}

	public void OnStartHover() {
		if(!clicked) {
			uiText.text = "Press " + CameraMovement.interactKey + " use button";
		}
	}
}
