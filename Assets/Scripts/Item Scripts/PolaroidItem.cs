using TMPro;
using UnityEngine;

public class PolaroidItem:MonoBehaviour, IInteractable {
	[SerializeField] private TextMeshProUGUI itemText;
	[SerializeField] private GameObject itemUI;
	public float MaxRange { get { return maxRange; } }
	private const float maxRange = 100f;
	public void OnEndHover() {
		itemText.text = "";
	}

	// DO NOT REMOVE THIS, WLL NOT WORK!
	public void OnInteract() {

	}

	public void OnStartHover() {
		itemText.text = "Press " + CameraMovement.interactKey + " to view";
	}
	private void Start() {
		itemUI = GameObject.Find("ItemUI");
		itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
	}
}
