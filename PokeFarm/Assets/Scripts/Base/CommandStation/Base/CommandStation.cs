using Base.CommandStation.Commands;
using Base.Managers;
using UnityEngine;

public class CommandStation : MonoBehaviour, IInteractable
{
    public bool isCanInteract => true;

    private CommandDataSO[] _allMonstersCommands;
    private Monster[] _allMonstersOnTheFarm;
    public Command CurrentCommand { get;  private set; }
    [SerializeField] private CommandStationUI _usingInterface;
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private MonsterCommandStationButton _monsterButtonPrefab;
    
    private bool isInited;

    //public static UnityEvent<MonstersInteractionWay> OnCommandChangedEvent = new UnityEvent<MonstersInteractionWay>();

    private void Start()
    {
        PrepareToInit();
        _usingInterface.gameObject.SetActive(false);
    }

    private void PrepareToInit()
    {
        _allMonstersOnTheFarm = MonstersManager.Instance.AllMonstersOnTheFarm.ToArray();
        _allMonstersCommands = MonstersManager.Instance.AllMonstersCommand;
        
        foreach (var command in _allMonstersCommands)
        {
            var button = _commandButtonPrefab;
            button.commandDataSO = command;
            button.Command = command.Command;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.CommandList.transform);
        }
        
        foreach (var monster in _allMonstersOnTheFarm)
        {
            var button = _monsterButtonPrefab;
            button.MonsterData = monster.MonsterData;
            button.CommandDataSO = monster.MonsterData.CommandData;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.MonstersList.transform);
        }
    }
    private void Init(Command command)
    {
        CurrentCommand = command;
        isInited = true;
    }

    public void Interact()
    {
        if (isInited)
        {
            ShowInterface();
        }
        ShowInitInterface();
    }

    private void ShowInitInterface()
    {
        _usingInterface.gameObject.SetActive(true);
    }

    private void ShowInterface()
    {
        _usingInterface.gameObject.SetActive(true);
    }
    
    private void HideInterface()
    {
        _usingInterface.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(ChangeCurrentCommand);
        MonsterCommandStationButton.OnCommandExecutedEvent.AddListener(HideInterface);
    }

    private void ChangeCurrentCommand(Command selectedCommand)
    {
        //CurrentCommand = selectedCommand;

        //OnCommandChangededEvent.Invoke(CurrentCommand);

    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(ChangeCurrentCommand);
        MonsterCommandStationButton.OnCommandExecutedEvent.RemoveListener(HideInterface);
    }
}
