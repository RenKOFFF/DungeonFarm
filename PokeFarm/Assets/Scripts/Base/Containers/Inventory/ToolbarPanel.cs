using UnityEngine;
using UnityEngine.EventSystems;

public class ToolbarPanel : ItemPanel
{
    private int currentSelectedItemIndex;

    private void Start()
    {
        Init();
        Hightlight(currentSelectedItemIndex);

        ToolbarController.instanse.OnSelectChangeEvent.AddListener(Hightlight);
    }
    public override void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        ToolbarController.instanse.Set(id);
    }

    private void Hightlight(int id)
    {
        buttons[currentSelectedItemIndex].Highlight(false);
        currentSelectedItemIndex = id;
        buttons[id].Highlight(true);
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
