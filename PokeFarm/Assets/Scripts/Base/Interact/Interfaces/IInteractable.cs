using UnityEngine;

public interface IInteractable
{
    GameObject gameObject { get; }
    bool isCanInteract { get; }
    void Interact();
}
