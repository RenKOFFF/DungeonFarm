using System;
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
    public static UnityEvent<IInteractable> OnPlayerIntactedEvent = new UnityEvent<IInteractable>();

    private void Awake() 
    {
        OnPlayerOnIntacteTriggerEvent.AddListener(CanInteract);
        OnPlayerExitIntacteTriggerEvent.AddListener(CanNotInteract);
        OnPlayerIntactedEvent.AddListener(CanInteract);

        if (ConditionalInteracteItem.IsInitialized)
            ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(CanInteract);
        else 
            ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.AddListener(WaitConditionalInteracteItemInitialize);
    }

    private void WaitConditionalInteracteItemInitialize()
    {
        ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(CanInteract);
        ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.RemoveListener(WaitConditionalInteracteItemInitialize);
    }

    private void CanInteract(IInteractable interactableObject)
    {
        bool interactableCondition = true;
        if (interactableObject is IConditionInteractable conditionInteractableObject)
        {
            interactableCondition = conditionInteractableObject.Condition;

            if (!ConditionalInteracteItem.IsInitialized || ConditionalInteracteItem.Item != conditionInteractableObject)
                ConditionalInteracteItem.Initialize(conditionInteractableObject);
        }

        isCanPlayerInteract = interactableObject.isCanInteract && interactableCondition;

        if (isCanPlayerInteract)
            this.interactableObject = interactableObject;
        
    }

    private void CanNotInteract()
    {
        isCanPlayerInteract = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isCanPlayerInteract)
        {
            interactableObject.Interact();
            OnPlayerIntactedEvent.Invoke(interactableObject);
        }   
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        collidersList.Add(other);

        var interactableObject = other.GetComponent<IInteractable>();
        if (interactableObject != null && interactableObject.isCanInteract)
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
