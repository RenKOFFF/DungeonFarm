using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterInteractionButton : MonoBehaviour
{
    public Button Button { get; private set; }
    [HideInInspector] public MonstersInteractionWayDataSO InteractData;
    [HideInInspector] public MonstersInteractionWay InteractionWay;

    public static UnityEvent OnInteractedEvent = new UnityEvent();

    private Image _icon;
    private TextMeshProUGUI _name;

    private bool _isInited;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        Button.onClick.AddListener(Interact);
        
    }

    private void Start()
    {
        Init();
        RefreshButtonData();
        //_button.interactable = false;
    }

    private void Init()
    {
        _icon = GetComponent<Image>();
        _name = GetComponentInChildren<TextMeshProUGUI>();
        _isInited = true;
    }

    public void Interact()
    {
        InteractionWay.gameObject.SetActive(true);
        InteractionWay.Execute();
        OnInteractedEvent.Invoke();
    }

    public void RefreshButtonData()
    {
        if (!_isInited) Init();

        if (InteractData != null)
        {
            _icon.sprite = InteractData.Icon;
            _name.text = InteractData.InteractName;
        }
        else
        {
            _icon.color = Color.red;
            _name.text = "Default";
        }
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(Interact);
    }
}
