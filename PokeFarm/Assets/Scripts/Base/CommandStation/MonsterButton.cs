using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MonsterButton : MonoBehaviour
{
    private Button _button;
    [HideInInspector] public MonsterDataSO MonsterData;
    [HideInInspector] public MonstersInteractionWayDataSO[] InteractionWays;

    //public static UnityEvent<MonstersInteractionWay> OnCommandSelectedEvent = new UnityEvent<MonstersInteractionWay>();

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    //[SerializeField] private TextMeshProUGUI _discription;

    private bool _isInited;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(Interact);
        CommandButton.OnCommandSelectedEvent.AddListener(SelectMonstersByCommand);
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
        //InteractionWay.gameObject.SetActive(true);
        //InteractionWay.Execute();
        //OnCommandSelectedEvent.Invoke(InteractionWay);
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
            //_discription.text = "DefaultDescr";
        }
    }

    private void SelectMonstersByCommand(MonstersInteractionWay command)
    {
        if (InteractionWays.ToList().Find(arr => arr == command.MonstersInteractionWayData))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CommandButton.OnCommandSelectedEvent.RemoveListener(SelectMonstersByCommand);
        _button.onClick.RemoveListener(Interact);
    }
}
