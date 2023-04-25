using TMPro;
using UnityEngine;

public class PlayerActions:MonoBehaviour {
	[Header("Player camera orientation and reach")]
	[SerializeField] public float reach = 4f;
	[SerializeField] private Transform playerCamera;

	[Header("Animators")]
	private Animator doorAnimator;

	[Header("Controls")]
	[SerializeField] private KeyCode actionButton = KeyCode.E;
	[SerializeField] public static KeyCode useCameraButton = KeyCode.Mouse0;

	[Header("Door specific variables")]
	[SerializeField] private GameObject thisDoor;
	private RaycastHit hitObject;
	private bool itemObject = false;
	private bool isDoorOpenFront = false;
	private bool isDoorOpenBack = false;
	public RuntimeAnimatorController doorAnimatorController;

	[Header("Polaroid")]
	[SerializeField] private TextMeshProUGUI uiText;

	private void Start() {
		doorAnimator = thisDoor.GetComponent<Animator>();
		doorAnimator.runtimeAnimatorController = doorAnimatorController;
	}

	private void Update() {
		itemObject = Physics.Raycast(playerCamera.position, playerCamera.forward, out hitObject, 6f);
		if(itemObject && hitObject.collider.gameObject.layer == LayerMask.NameToLayer("Front Door")) {
			//uiText.text = "Press " + actionButton + " to use";
			if(Input.GetKeyDown(actionButton))
				Door();
		} else {
			//uiText.text = "";
		}

		if(itemObject && hitObject.collider.gameObject.CompareTag("Polaroid")) {
			uiText.text = "Press " + actionButton + " to gamble";
			if(Input.GetKeyDown(actionButton)) {
				GamblePolaroid();
			}
		} else {
			uiText.text = "";
		}
	}
	private void Door() {
		if(hitObject.collider.gameObject.layer == LayerMask.NameToLayer("Front Door")) {
			if(!isDoorOpenFront && !isDoorOpenBack) {
				this.doorAnimator.SetBool("OpenDoor", true);
				this.doorAnimator.SetBool("CloseDoor", false);
				this.doorAnimator.SetBool("CloseDoor2", false);
				isDoorOpenFront = true;
			} else if(isDoorOpenFront) {
				this.doorAnimator.SetBool("CloseDoor", true);
				this.doorAnimator.SetBool("OpenDoor", false);
				isDoorOpenFront = false;
			} else {
				this.doorAnimator.SetBool("CloseDoor2", true);
				this.doorAnimator.SetBool("OpenDoor", false);
				this.doorAnimator.SetBool("OpenDoor", false);
				isDoorOpenFront = false;
			}
			doorAnimator.SetBool("OpenDoor2", false);
			isDoorOpenBack = false;

		} else if(hitObject.collider.gameObject.layer == LayerMask.NameToLayer("Back Door")) {
			if(!isDoorOpenBack && !isDoorOpenFront) {
				doorAnimator.SetBool("OpenDoor2", true);
				doorAnimator.SetBool("CloseDoor", false);
				doorAnimator.SetBool("CloseDoor2", false);
				isDoorOpenBack = true;
			} else if(isDoorOpenBack) {
				doorAnimator.SetBool("CloseDoor2", true);
				doorAnimator.SetBool("OpenDoor2", false);

				isDoorOpenBack = false;
			} else {
				doorAnimator.SetBool("CloseDoor", true);
				doorAnimator.SetBool("OpenDoor2", false);
				isDoorOpenBack = false;
			}
			doorAnimator.SetBool("OpenDoor", false);
			isDoorOpenFront = false;
		}
	}

	private void GamblePolaroid() {

	}
}
