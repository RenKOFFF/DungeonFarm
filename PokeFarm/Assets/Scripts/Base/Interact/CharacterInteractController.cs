using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInteractController : MonoBehaviour
{
    private bool isCanPlayerInteract;
    private IInteractable interactableObject;

    List<Collider2D> collidersList = new List<Collider2D>();

    public static UnityEvent<IInteractable> OnPlayerOnIntacteTriggerEvent = new UnityEvent<IInteractable>();
    public static UnityEvent OnPlayerExitIntacteTriggerEvent = new UnityEvent();

    private void Awake() 
    {
        OnPlayerOnIntacteTriggerEvent.AddListener(CanInteract);
        OnPlayerExitIntacteTriggerEvent.AddListener(CanNotInteract);
    }

    private void CanInteract(IInteractable interactableObject)
    {
        this.interactableObject = interactableObject;
        isCanPlayerInteract = true;
    }

    private void CanNotInteract()
    {
        isCanPlayerInteract = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isCanPlayerInteract)
        {
            Debug.Log("Interact EEEEEE");
            interactableObject.Interact();
        }   
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        collidersList.Add(other);

        Debug.Log("On Collider " + other.name);
        var interactableObject = other.GetComponent<IInteractable>();
        if (interactableObject != null)
        {
            OnPlayerOnIntacteTriggerEvent.Invoke(interactableObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collidersList.Remove(other);

        if (other.GetComponent<IInteractable>() != null && collidersList.Count == 0)
        {
            OnPlayerExitIntacteTriggerEvent.Invoke();
        }
    }
}
