using Base.CommandStation.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private Button _button;
    [HideInInspector] public CommandDataSO commandDataSO;
    [HideInInspector] public Command Command;

    public static UnityEvent<Command> OnCommandSelectedEvent = new();

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _discription;

    private bool _isInited;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(Interact);
    }

    private void Start()
    {
        Init();
        RefreshButtonData();
    }

    private void Init()
    {
        //_icon = GetComponentInChildren<Image>();
        //_name = GetComponentInChildren<TextMeshProUGUI>();
        //_discription = GetComponentInChildren<TextMeshProUGUI>();

        _isInited = true;
    }

    public void Interact()
    {
        //InteractionWay.gameObject.SetActive(true);
        //InteractionWay.Execute();
        OnCommandSelectedEvent.Invoke(Command);
    }

    public void RefreshButtonData()
    {
        if (!_isInited) Init();

        if (commandDataSO != null)
        {
            //_icon.sprite = commandDataSO.Icon;
            _name.text = commandDataSO.CommandName;
            _discription.text = "-----------";
        }
        else
        {
            _icon.color = Color.red;
            _name.text = "Default";
            _discription.text = "DefaultDescription";
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Interact);
    }
}
