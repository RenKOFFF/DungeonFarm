using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolbarPanel : ItemPanel
{
    private int currentSelectedItemIndex;
    [SerializeField] ToolbarController toolbarController;

    private void Start()
    {
        Init();
        Hightlight(currentSelectedItemIndex);
        toolbarController.OnSelectChangeEvent.AddListener(Hightlight);
    }
    public override void OnClick(int id)
    {
        toolbarController.Set(id);
        Hightlight(id);
    }

    private void Hightlight(int id)
    {
        buttons[currentSelectedItemIndex].Hightlight(false);
        currentSelectedItemIndex = id;
        buttons[id].Hightlight(true);
    }
}
