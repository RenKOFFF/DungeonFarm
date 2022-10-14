using UnityEngine;
using TMPro;

public class InteractUiTextHandler : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Start() {
        textUI = GetComponent<TextMeshProUGUI>();
        HideText();
    }
    private void OnEnable() 
    {
        CharacterInteractController.OnPlayerOnIntacteTriggerEvent.AddListener(UpdateText);
        CharacterInteractController.OnPlayerExitIntacteTriggerEvent.AddListener(HideText);
        CharacterInteractController.OnPlayerIntactedEvent.AddListener(UpdateText);

        if (ConditionalInteracteItem.IsInitialized)
            ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(UpdateText);
        else
            ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.AddListener(WaitConditionalInteracteItemInitialize);
    }

    private void UpdateText(IInteractable interactableObject)
    {
        if (!CharacterInteractController.IsSomeoneItemsInPlayerCollider) return;

        if (!interactableObject.isCanInteract)
        {
            HideText();
            return;
        }

        if (interactableObject is IConditionInteractable conditionInteractableObject)
        {
            var interactableCondition = conditionInteractableObject.Condition;

            /*if (!ConditionalInteracteItem.IsInitialized || ConditionalInteracteItem.Item != conditionInteractableObject)
                ConditionalInteracteItem.Initialize(conditionInteractableObject);*/

            if (!interactableCondition)
            {
                HideText();
                return;
            }
        }

        gameObject.SetActive(true);
        textUI.text = $"Press \"E\" to Interact with {interactableObject}";
    }
    private void HideText()
    {
        gameObject.SetActive(false);
    }
    private void WaitConditionalInteracteItemInitialize()
    {
        ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(UpdateText);
        ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.RemoveListener(WaitConditionalInteracteItemInitialize);
    }
}
