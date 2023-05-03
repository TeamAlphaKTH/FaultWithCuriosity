using TMPro;
using UnityEngine;

public class ButtonController:MonoBehaviour, IInteractable {
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 1f;
	private TextMeshProUGUI uiText;
	private Animator animator;

	private bool clicked = false;
	/// <summary>
	/// Make the button only clickable once. It can not be reset.
	/// </summary>
	[SerializeField] bool clickOnce = false;

	// Start is called before the first frame update
	void Start() {
		uiText = GameObject.Find("ItemUI").GetComponentInChildren<TextMeshProUGUI>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		//Update the animations
		if(clicked) {
			animator.SetBool("Clicked", true);
		} else if(animator.GetBool("Clicked")) {
			animator.SetBool("Clicked", false);
		}
	}

	public void OnEndHover() {
		uiText.text = "";
	}

	public void OnInteract() {
		//Update the state depending on the clicked state
		if(!clicked) {
			clicked = true;
			if(clickOnce) {
				OnEndHover();
			} else {
				OnStartHover();
			}
		} else if(!clickOnce) {
			clicked = false;
			OnStartHover();
		}
	}

	public void OnStartHover() {
		//Change text depending on the clicked state
		if(!clicked) {
			uiText.text = "Press " + CameraMovement.interactKey + " use button";
		} else if(clicked && !clickOnce) {
			uiText.text = "Press " + CameraMovement.interactKey + " reset button";
		}
	}
}
