using TMPro;
using UnityEngine;

public class Note:MonoBehaviour, IInteractable {
    public float MaxRange { get { return maxRange; } }
    private const float maxRange = 2f;

    private TextMeshProUGUI itemText;
    private GameObject itemUI;
    [SerializeField] private GameObject noteUI;
    [SerializeField] private Canvas canvas;
    private bool isOn = false;

    public void OnEndHover() {
        itemText.text = "";
    }

    public void OnInteract() {
        isOn = !isOn;
        noteUI.SetActive(true);
    }

    public void OnStartHover() {
        itemText.text = "Press " + CameraMovement.interactKey + " to read note";
    }

    // Start is called before the first frame update
    void Start() {
        itemUI = GameObject.Find("ItemUI");
        itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();

        canvas = transform.parent.GetChild(1).GetComponentInChildren<Canvas>();
    }
    void Update() {
        if(isOn && Input.GetKeyDown(KeyCode.Escape)) {
            noteUI.SetActive(false);
        }
    }
}
