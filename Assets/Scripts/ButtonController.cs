using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ButtonController:NetworkBehaviour, IInteractable {
    public float MaxRange { get { return maxRange; } }
    private const float maxRange = 2f;
    private TextMeshProUGUI uiText;
    private Animator animator;
    [SerializeField] public AudioSource leverSound;
    private float soundDelay = 0.1f;
    private float soundBackDelay = 0.04f;


    public bool clicked = false;
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
            AnimateButtonServerRpc(true);
        } else if(animator.GetBool("Clicked")) {
            AnimateButtonServerRpc(false);
        }
    }

    public void OnEndHover() {
        uiText.text = "";
    }

    public void OnInteract() {
        //Update the state depending on the clicked state
        if(!clicked) {
            UpdateButtonServerRpc(true);
            if(clickOnce) {
                OnEndHover();
                leverSound.PlayDelayed(soundDelay);
            } else {
                OnStartHover();
                leverSound.PlayDelayed(soundDelay);
            }
        } else if(!clickOnce) {
            UpdateButtonServerRpc(false);
            OnStartHover();
            leverSound.PlayDelayed(soundBackDelay);
        }
    }

    public void OnStartHover() {
        //Change text depending on the clicked state
        if(!clicked) {
            uiText.text = "Press " + CameraMovement.interactKey + " to interact";
        } else if(clicked && !clickOnce) {
            uiText.text = "Press " + CameraMovement.interactKey + " reset button";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AnimateButtonServerRpc(bool state) {
        AnimateButtonClientRpc(state);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateButtonServerRpc(bool state) {
        UpdateButtonClientRpc(state);
    }

    [ClientRpc]
    private void AnimateButtonClientRpc(bool state) {
        animator.SetBool("Clicked", state);
    }

    [ClientRpc]
    private void UpdateButtonClientRpc(bool state) {
        clicked = state;
    }
}

