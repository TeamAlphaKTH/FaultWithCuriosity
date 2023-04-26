public interface IInteractable {
    float MaxRange { get; }

    // Do not write public/private, it is forced already.

    void OnStartHover();
    void OnInteract();
    void OnEndHover();
}
