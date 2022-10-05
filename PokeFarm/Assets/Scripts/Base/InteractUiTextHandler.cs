using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void ShowText(IInteractable interactableObject)
    {
        gameObject.SetActive(true);
        textUI.text = $"Press \"E\" to Interact with {interactableObject}";
    }

    private void HideText()
    {
        gameObject.SetActive(false);
    }
}
