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
    [SerializeField] private CommandStationMonsterButton _commandStationMonsterButtonPrefab;

    public bool IsInited { get; private set; }
    private static bool _isPrepareToInitEnded;

    //public static UnityEvent<MonstersInteractionWay> OnCommandChangedEvent = new UnityEvent<MonstersInteractionWay>();

    private void Start()
    {
        Interact();
    }

    private void PrepareToInit()
    {
        if(_isPrepareToInitEnded) return;
        
        _usingInterface = CommandStationUI.Instance;//FindObjectOfType<CommandStationUI>();
        
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
            var button = _commandStationMonsterButtonPrefab;
            button.MonsterData = monster.MonsterData;
            button.CommandDataSO = monster.MonsterData.CommandData;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.MonstersList.transform);

            _isPrepareToInitEnded = true;
        }
    }
    private void Init(Command command)
    {
        CurrentCommand = command;
        IsInited = true;
    }

    public void Interact()
    {
        if (!IsInited)
        {
            PrepareToInit();
            ShowInitInterface();
        }
        
        ShowInterface();
    }

    private void ShowInitInterface()
    {
        _usingInterface.CommandList.gameObject.SetActive(true);
        _usingInterface.gameObject.SetActive(true);
    }

    private void ShowInterface()
    {
        //_usingInterface.CommandList.gameObject.SetActive(false);
        _usingInterface.gameObject.SetActive(true);
    }
    
    private void HideInterface()
    {
        _usingInterface.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(Init);
        CommandStationMonsterButton.OnCommandExecutedEvent.AddListener(HideInterface);
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(Init);
        CommandStationMonsterButton.OnCommandExecutedEvent.RemoveListener(HideInterface);
    }
}
