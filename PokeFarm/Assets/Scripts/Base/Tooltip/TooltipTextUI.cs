using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Text => $"Бесомон\nИмя: {_button.MonsterData.MonsterName}";
    private CommandStationMonsterButton _button;

    private void Start()
    {
        _button = GetComponent<CommandStationMonsterButton>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData e)
    {
        Tooltip.text = Text;
        Tooltip.isUI = true;
    }
	
    void IPointerExitHandler.OnPointerExit(PointerEventData e)
    {
        Tooltip.isUI = false;
    }
}