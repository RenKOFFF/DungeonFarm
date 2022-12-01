using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private Button _button;
    [HideInInspector] public MonstersInteractionWayDataSO InteractData;
    [HideInInspector] public MonstersInteractionWay InteractionWay;

    public static UnityEvent<MonstersInteractionWay> OnCommandSelectedEvent = new UnityEvent<MonstersInteractionWay>();

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
        OnCommandSelectedEvent.Invoke(InteractionWay);
    }

    public void RefreshButtonData()
    {
        if (!_isInited) Init();

        if (InteractData != null)
        {
            _icon.sprite = InteractData.Icon;
            _name.text = InteractData.InteractName;
            _discription.text = "abra barabra";
        }
        else
        {
            _icon.color = Color.red;
            _name.text = "Default";
            _discription.text = "DefaultDescr";
        }
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Interact);
    }
}
