using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInteractionButton : MonoBehaviour
{
    public MonstersInteractionWayDataSO InteractData;

    private Image _icon;
    private TextMeshProUGUI _name;

    private bool _isInited;

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
