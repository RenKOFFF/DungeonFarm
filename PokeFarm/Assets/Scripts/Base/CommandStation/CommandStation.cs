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
    public MonsterDataSO[] _allMonstersOnTheFarm;
    public MonstersInteractionWay CurrentCommand { get;  private set; }
    [SerializeField] private CommandStationUI _usingInterface;
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private MonsterButton _monsterButtonPrefab;

    public static UnityEvent<MonstersInteractionWay> OnCommandChangededEvent = new UnityEvent<MonstersInteractionWay>();

    private void Awake()
    {
        Init();
        _usingInterface?.gameObject.SetActive(false);
    }

    private void Init()
    {
        _allMonstersInteractionWays = Resources.LoadAll<MonstersInteractionWayDataSO>("Monsters/InteractionsWays");

        foreach (var command in _allMonstersInteractionWays)
        {
            var button = _commandButtonPrefab;
            button.InteractData = command;
            button.InteractionWay = command.InteractionWay;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.CommandList.transform);
        }

        // TODO сделать менеджер в котором будут храниться все монстры
        //_allMonstersOnTheFarm = сюда напиши то что сверху;

        _allMonstersOnTheFarm = Resources.LoadAll<MonsterDataSO>("Monsters");

        foreach (var monster in _allMonstersOnTheFarm)
        {
            var button = _monsterButtonPrefab;
            button.MonsterData = monster;
            button.InteractionWays = monster.InteractionWay;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.MonstersList.transform);
        }
    }

    public void Interact()
    {
        ShowInterface();
    }

    private void ShowInterface()
    {
        _usingInterface?.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(ChangeCurrentCommand);
    }

    private void ChangeCurrentCommand(MonstersInteractionWay selectedCommand)
    {
        CurrentCommand = selectedCommand;

        OnCommandChangededEvent.Invoke(CurrentCommand);

    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(ChangeCurrentCommand);
    }
}
