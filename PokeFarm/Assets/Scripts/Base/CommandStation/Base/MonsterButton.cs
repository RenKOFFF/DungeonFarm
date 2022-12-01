using System.Linq;
using Base.CommandStation.Commands;
using Base.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterButton : MonoBehaviour
{
    private Button _button;
    [HideInInspector] public MonsterDataSO MonsterData;
    [HideInInspector] public CommandDataSO[] CommandDataSO;

    public static UnityEvent OnCommandExecutedEvent = new ();

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    //[SerializeField] private TextMeshProUGUI _discription;

    private bool _isInited;

    private Command _command;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(Interact);
        CommandButton.OnCommandSelectedEvent.AddListener(SelectMonstersByCommand);
        OnCommandExecutedEvent.AddListener(ReturnInteract);
    }

    private void Start()
    {
        Init();
        RefreshButtonData();
    }

    private void Init()
    {
        //_icon = GetComponent<Image>();
        //_name = GetComponentInChildren<TextMeshProUGUI>();
        //_discription = GetComponentInChildren<TextMeshProUGUI>();

        _isInited = true;
    }

    public void Interact()
    {
        _command.CurrentMonster = MonstersManager.Instance.GetMonsterInstance(MonsterData);
        _command.Execute();
        OnCommandExecutedEvent.Invoke();
    }

    public void RefreshButtonData()
    {
        if (!_isInited) Init();

        if (MonsterData != null)
        {
            _icon.sprite = MonsterData.Icon;
            _name.text = MonsterData.MonsterName;
            //_discription.text = "abra barabra";
        }
        else
        {
            _icon.color = Color.red;
            _name.text = "Default";
            //_discription.text = "DefaultDescription";
        }
    }

    private void SelectMonstersByCommand(Command command)
    {
        if (CommandDataSO.ToList().Find(arr => arr == command.CommandDataSO))
        {
            _button.interactable = true;
            //gameObject.SetActive(true);
            _command = command;
        }
        else
        {
            _command = null;
            _button.interactable = false;
            //gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(SelectMonstersByCommand);
        _button.onClick.RemoveListener(Interact);
        OnCommandExecutedEvent.AddListener(ReturnInteract);
    }
    
    private void ReturnInteract()
    {
        //_button.interactable = true;
    }
}
