using UnityEngine;

public class PlayerActions:MonoBehaviour {
    [Header("Player camera orientation and reach")]
    [SerializeField] private float reach = 4f;
    [SerializeField] private Transform playerCamera;

    [Header("Animators")]
    private Animator doorAnimator;

    [Header("Controls")]
    [SerializeField] private KeyCode actionButton = KeyCode.E;

    [Header("Door specific variables")]
    [SerializeField] private GameObject thisDoor;
    private RaycastHit hitDoor;
    private bool isDoor = false;
    private bool isDoorOpenFront = false;
    private bool isDoorOpenBack = false;
    public RuntimeAnimatorController doorAnimatorController;

    private void Start() {
        doorAnimator = thisDoor.GetComponent<Animator>();
        doorAnimator.runtimeAnimatorController = doorAnimatorController;
    }

    private void Update() {
        isDoor = Physics.Raycast(playerCamera.position, playerCamera.forward, out hitDoor, reach);
        if(isDoor && Input.GetKeyDown(actionButton))
            Door();
    }
    private void Door() {
        if(hitDoor.collider.gameObject.layer == LayerMask.NameToLayer("Front Door")) {
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

        } else if(hitDoor.collider.gameObject.layer == LayerMask.NameToLayer("Back Door")) {
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
}
