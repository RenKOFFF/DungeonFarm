using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandStation : MonoBehaviour, IInteractable
{
    public bool isCanInteract => true;

    public MonstersInteractionWay CurrentCommand { get;  private set; }

    public UnityEvent<MonstersInteractionWay> OnCommandSelectedEvent = new UnityEvent<MonstersInteractionWay>();

    public void Interact()
    {
        ShowInterface();
    }

    private void ShowInterface()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        OnCommandSelectedEvent.AddListener(ChangeCurrentCommand);
    }

    private void ChangeCurrentCommand(MonstersInteractionWay selectedCommand)
    {
        CurrentCommand = selectedCommand;
    }
}
