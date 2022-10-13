using UnityEngine;
using TMPro;

public class InteractUiTextHandler : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Start() {
        HideText();
        textUI = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable() 
    {
        CharacterInteractController.OnPlayerOnIntacteTriggerEvent.AddListener(ShowText);
        CharacterInteractController.OnPlayerExitIntacteTriggerEvent.AddListener(HideText);
        CharacterInteractController.OnPlayerIntactedEvent.AddListener(ShowText);

        if (ConditionalInteracteItem.IsInitialized)
            ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(ShowText);
        else
            ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.AddListener(WaitConditionalInteracteItemInitialize);
    }

    private void WaitConditionalInteracteItemInitialize()
    {
        ConditionalInteracteItem.Item.OnConditionUpdatedEvent.AddListener(ShowText);
        ConditionalInteracteItem.OnConditionalInteracteItemInitializedEvent.RemoveListener(WaitConditionalInteracteItemInitialize);
    }

    private void ShowText(IInteractable interactableObject)
    {
        if (!interactableObject.isCanInteract)
        {
            HideText();
            return;
        }

        if (interactableObject is IConditionInteractable conditionInteractableObject)
        {
            var interactableCondition = conditionInteractableObject.Condition;

            if (!ConditionalInteracteItem.IsInitialized || ConditionalInteracteItem.Item != conditionInteractableObject)
                ConditionalInteracteItem.Initialize(conditionInteractableObject);

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
}
