using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolbarPanel : ItemPanel
{
    private int currentSelectedItemIndex;

    private void Start()
    {
        Init();
        Hightlight(currentSelectedItemIndex);

        ToolbarController.instanse.OnSelectChangeEvent.AddListener(Hightlight);
    }
    public override void OnClick(int id)
    {
        ToolbarController.instanse.Set(id);
    }

    private void Hightlight(int id)
    {
        buttons[currentSelectedItemIndex].Hightlight(false);
        currentSelectedItemIndex = id;
        buttons[id].Hightlight(true);
    }
    public Item GetCurrentSelectedItem()
    {
        var returnItem = buttons[currentSelectedItemIndex].GetSlot()?.item;
        Debug.Log($"Current item: {returnItem} on slot index: {currentSelectedItemIndex}");
        return returnItem;
    }
    public Item GetCurrentSelectedItem(int index)
    {
        var returnItem = buttons[index].GetSlot()?.item;
        Debug.Log($"Current item: {returnItem} on slot index: {index}");
        return returnItem;
    }
}
