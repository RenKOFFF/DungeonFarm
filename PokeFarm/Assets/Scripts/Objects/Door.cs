using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject open, closed;
    protected bool isOpen;

    public bool isCanInteract { get => _isCanInteract; private set => _isCanInteract = value; }
    private bool _isCanInteract = true;
    public virtual void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
        }
        else
        {
            isOpen = false;
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
        }
    }
}
