using System;
using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandStationUI : MonoBehaviour
{
    public static UnityEvent<bool> IsUiActiveEvent = new();
    [field : SerializeField] public GridLayoutGroup CommandList { get; private set; }
    [field: SerializeField] public GridLayoutGroup MonstersList { get; private set; }

    public void SetDefaultState()
    {
        CommandList.gameObject.SetActive(true);
    }

    private void HideCommand(Command _)
    {
        CommandList.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        IsUiActiveEvent.Invoke(true);
        Debug.Log($"IsUiActiveEvent.Invoke(true)");
        CommandButton.OnCommandSelectedEvent.AddListener(HideCommand);
        //MonsterCommandStationButton.OnCommandExecutedEvent.AddListener(HideMonstersAndShowCommand);
    }
    
    private void OnDisable()
    {
        IsUiActiveEvent.Invoke(false);
        Debug.Log($"IsUiActiveEvent.Invoke(false)");
        CommandButton.OnCommandSelectedEvent.RemoveListener(HideCommand);
        //MonsterCommandStationButton.OnCommandExecutedEvent.RemoveListener(HideMonstersAndShowCommand);
    }
}
