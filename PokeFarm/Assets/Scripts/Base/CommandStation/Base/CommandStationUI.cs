using System;
using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.UI;

public class CommandStationUI : MonoBehaviour
{
    public static CommandStationUI Instance;
    [field : SerializeField] public GridLayoutGroup CommandList { get; private set; }
    [field: SerializeField] public GridLayoutGroup MonstersList { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(HideCommandAndShowMonsters);
        //MonsterCommandStationButton.OnCommandExecutedEvent.AddListener(HideMonstersAndShowCommand);
    }
    
    public void SetDefaultState()
    {
        CommandList.gameObject.SetActive(true);
    }

    private void HideCommandAndShowMonsters(Command _)
    {
        CommandList.gameObject.SetActive(false);
        //MonstersList.gameObject.SetActive(true);
    }
    
    private void HideMonstersAndShowCommand()
    {
        //CommandList.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(HideCommandAndShowMonsters);
        //MonsterCommandStationButton.OnCommandExecutedEvent.RemoveListener(HideMonstersAndShowCommand);
    }
}
