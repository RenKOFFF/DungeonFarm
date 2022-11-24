using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandStation : MonoBehaviour, IInteractable
{
    public bool isCanInteract => true;

    private MonstersInteractionWayDataSO[] _allMonstersInteractionWays;
    public MonstersInteractionWay CurrentCommand { get;  private set; }
    [SerializeField] private GameObject _usingInterface;
    [SerializeField] private CommandButton _commandButtonPrefab;

    [HideInInspector] public UnityEvent<MonstersInteractionWay> OnCommandSelectedEvent = new UnityEvent<MonstersInteractionWay>();

    private void Awake()
    {
        Init();
        _usingInterface?.SetActive(false);
    }

    private void Init()
    {
        _allMonstersInteractionWays = Resources.LoadAll<MonstersInteractionWayDataSO>("Monsters/InteractionsWays");

        var commandList = _usingInterface.GetComponentInChildren<GridLayoutGroup>();

        foreach (var command in _allMonstersInteractionWays)
        {
            var button = _commandButtonPrefab;
            button.InteractData = command;
            button.InteractionWay = command.InteractionWay;
            button.RefreshButtonData();

            Instantiate(button, commandList.transform);
        }
    }

    public void Interact()
    {
        ShowInterface();
    }

    private void ShowInterface()
    {
        _usingInterface?.SetActive(true);
    }

    private void OnEnable()
    {
        OnCommandSelectedEvent.AddListener(ChangeCurrentCommand);
    }

    private void ChangeCurrentCommand(MonstersInteractionWay selectedCommand)
    {
        CurrentCommand = selectedCommand;
    }

    private void OnDisable()
    {
        OnCommandSelectedEvent.RemoveListener(ChangeCurrentCommand);
    }
}
