using Base.CommandStation.Commands;
using Base.Managers;
using UnityEngine;

public class CommandStation : MonoBehaviour, IInteractable
{
    public bool isCanInteract => true;

    private CommandDataSO[] _allMonstersCommands;
    private Monster[] _allMonstersOnTheFarm;
    public Command CurrentCommand { get;  private set; }
    
    private CommandStationUI _usingInterface;
    [SerializeField] private CommandStationUI _prefabUI;
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private CommandStationMonsterButton _commandStationMonsterButtonPrefab;


    private bool IsInitiated { get; set; }
    private bool _isPrepareToInitEnded;

    //public static UnityEvent<MonstersInteractionWay> OnCommandChangedEvent = new UnityEvent<MonstersInteractionWay>();

    private void Start()
    {
        Interact();
    }

    private void PrepareToInit()
    {
        if(_isPrepareToInitEnded) return;

        if (_usingInterface == null)
        {
            var canvas = GameObject.FindWithTag("Canvas");
            _usingInterface = canvas ? Instantiate(_prefabUI, canvas.transform) : Instantiate(_prefabUI);
        }
        
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
        IsInitiated = true;
    }

    public void Interact()
    {
        if (!IsInitiated)
        {
            PrepareToInit();
            //ShowInitInterface();
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
