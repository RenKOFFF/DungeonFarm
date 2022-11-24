using UnityEngine.EventSystems;

public class ToolbarPanel : ContainerPanel
{
    private int _currentSelectedItemIndex;

    public Item GetCurrentSelectedItem()
        => GetItemOnIndex(_currentSelectedItemIndex);

    private void HighlightSlotOnIndex(int index)
    {
        inventoryButtons[_currentSelectedItemIndex].Highlight(false);
        _currentSelectedItemIndex = index;
        inventoryButtons[index].Highlight(true);
    }

    private Item GetItemOnIndex(int index)
        => inventoryButtons[index].GetSlot()?.item;

    private new void Start()
    {
        base.Start();
        HighlightSlotOnIndex(_currentSelectedItemIndex);
        ToolbarManager.Instance.OnSelectedSlotIndexChanged.AddListener(HighlightSlotOnIndex);
    }

    public override void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        ToolbarManager.Instance.SetSlotIndex(id);
    }
}
