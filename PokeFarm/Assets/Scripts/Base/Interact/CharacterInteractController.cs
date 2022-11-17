using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInteractController : MonoBehaviour
{
    private bool isCanPlayerInteract;
    public static bool IsSomeoneItemsInPlayerCollider { get; private set; }

    private IInteractable currentInteractableObject;

    List<IInteractable> interactableObjectsList = new List<IInteractable>();

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

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isCanPlayerInteract)
        {
            if (currentInteractableObject == null) return;
                    
            currentInteractableObject.Interact();
            SwitchInteractableObject();

            OnPlayerIntactedEvent.Invoke(currentInteractableObject);
        }   
    }

    private void CanInteract(IInteractable interactableObject)
    {
        if (!IsSomeoneItemsInPlayerCollider || interactableObjectsList.Count == 0) return;

        bool interactableCondition = true;
        if (interactableObject is IConditionInteractable conditionInteractableObject)
        {
            interactableCondition = conditionInteractableObject.Condition;

            if (!ConditionalInteracteItem.IsInitialized || ConditionalInteracteItem.Item != conditionInteractableObject)
                ConditionalInteracteItem.Initialize(conditionInteractableObject);
        }

        isCanPlayerInteract = interactableObject.isCanInteract && interactableCondition;

        if (isCanPlayerInteract)
            currentInteractableObject = interactableObject;

    }

    private void CanNotInteract()
    {
        isCanPlayerInteract = false;
        currentInteractableObject = null;
    }

    private void SwitchInteractableObject()
    {
        if (interactableObjectsList.Count < 2) return;

        var lastIndexCurrentInteractableObject = interactableObjectsList.LastIndexOf(currentInteractableObject);

        lastIndexCurrentInteractableObject = lastIndexCurrentInteractableObject < 1 ? interactableObjectsList.Count - 1 : --lastIndexCurrentInteractableObject;

        currentInteractableObject = interactableObjectsList[lastIndexCurrentInteractableObject];
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var collider = other.GetComponent<IInteractable>();
        if (collider != null)
        {
            interactableObjectsList.Add(collider);
            IsSomeoneItemsInPlayerCollider = true;
            OnPlayerOnIntacteTriggerEvent.Invoke(interactableObjectsList[interactableObjectsList.Count - 1]);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var collider = other.GetComponent<IInteractable>();
        if (collider != null)
        {
            interactableObjectsList.Remove(collider);

            if (interactableObjectsList.Count == 0)
            {
                IsSomeoneItemsInPlayerCollider = false;
                OnPlayerExitIntacteTriggerEvent.Invoke();
            }
        }
    }
    private void WaitConditionalInteracteItemInitialize()
    {
        ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(CanInteract);
        ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.RemoveListener(WaitConditionalInteracteItemInitialize);
    }
}
