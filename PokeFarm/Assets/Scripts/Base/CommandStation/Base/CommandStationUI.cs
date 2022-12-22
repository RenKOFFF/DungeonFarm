using System;
using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.UI;

public class CommandStationUI : MonoBehaviour
{
    [field : SerializeField] public GridLayoutGroup CommandList { get; private set; }
    [field: SerializeField] public GridLayoutGroup MonstersList { get; private set; }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(HideCommand);
        //MonsterCommandStationButton.OnCommandExecutedEvent.AddListener(HideMonstersAndShowCommand);
    }
    
    public void SetDefaultState()
    {
        CommandList.gameObject.SetActive(true);
    }

    private void HideCommand(Command _)
    {
        CommandList.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(HideCommand);
        //MonsterCommandStationButton.OnCommandExecutedEvent.RemoveListener(HideMonstersAndShowCommand);
    }
}
