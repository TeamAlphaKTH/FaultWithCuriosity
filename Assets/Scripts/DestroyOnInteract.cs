using UnityEngine;

public class DestroyOnInteract:MonoBehaviour, IInteractable {

    public float MaxRange { get { return maxRange; } }

    private const float maxRange = 100f;

    public void OnStartHover() {
        Debug.Log($"Ready to destroy {gameObject.name}");
    }

    public void OnInteract() {
        Destroy(gameObject);
        Debug.Log("Destroyed!!!!");
    }

    public void OnEndHover() {
        Debug.Log($"We have lost the object");
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
