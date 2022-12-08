using Base.CommandStation.Commands;
using Base.Managers;
using UnityEngine;

public class CommandStation : MonoBehaviour, IInteractable
{
    public bool isCanInteract => true;

    private CommandDataSO[] _allMonstersCommands;
    private Monster[] _allMonstersOnTheFarm;
    //public Command CurrentCommand { get;  private set; }
    [SerializeField] private CommandStationUI _usingInterface;
    [SerializeField] private CommandButton _commandButtonPrefab;
    [SerializeField] private MonsterButton _monsterButtonPrefab;

    //public static UnityEvent<MonstersInteractionWay> OnCommandChangedEvent = new UnityEvent<MonstersInteractionWay>();

    private void Awake()
    {
        Init();
        _usingInterface?.gameObject.SetActive(false);
    }

    private void Init()
    {
        _allMonstersCommands = Resources.LoadAll<CommandDataSO>("Monsters/Commands");

        foreach (var command in _allMonstersCommands)
        {
            var button = _commandButtonPrefab;
            button.commandDataSO = command;
            button.Command = command.Command;
            button.RefreshButtonData();

            Instantiate(button, _usingInterface.CommandList.transform);
        }

        _allMonstersOnTheFarm = MonstersManager.Instance.AllMonstersOnTheFarm.ToArray();

        foreach (var monster in _allMonstersOnTheFarm)
        {
            var button = _monsterButtonPrefab;
            button.MonsterData = monster.MonsterData;
            button.CommandDataSO = monster.MonsterData.CommandData;
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
        _usingInterface.gameObject.SetActive(true);
    }
    
    private void HideInterface()
    {
        _usingInterface.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CommandButton.OnCommandSelectedEvent.AddListener(ChangeCurrentCommand);
        MonsterButton.OnCommandExecutedEvent.AddListener(HideInterface);
    }

    private void ChangeCurrentCommand(Command selectedCommand)
    {
        //CurrentCommand = selectedCommand;

        //OnCommandChangededEvent.Invoke(CurrentCommand);

    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(ChangeCurrentCommand);
        MonsterButton.OnCommandExecutedEvent.RemoveListener(HideInterface);
    }
}
