using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterInteractionButton : MonoBehaviour
{
    private Button _button;
    [HideInInspector] public MonstersInteractionWayDataSO InteractData;
    [HideInInspector] public MonstersInteractionWay InteractionWay;

    //public static UnityEvent OnButtonPressedEvent = new UnityEvent();

    private Image _icon;
    private TextMeshProUGUI _name;

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
        _icon = GetComponent<Image>();
        _name = GetComponentInChildren<TextMeshProUGUI>();
        _isInited = true;
    }

    public void Interact()
    {
        InteractionWay.gameObject.SetActive(true);
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
}
