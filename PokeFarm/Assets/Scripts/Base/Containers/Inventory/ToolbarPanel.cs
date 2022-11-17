using UnityEngine.EventSystems;

public class ToolbarPanel : ItemPanel
{
    private int _currentSelectedItemIndex;

    public override void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        ToolbarManager.Instance.SetSlotIndex(id);
    }

    public Item GetCurrentSelectedItem()
        => GetItemOnIndex(_currentSelectedItemIndex);

    private void HighlightSlotOnIndex(int index)
    {
        buttons[_currentSelectedItemIndex].Highlight(false);
        _currentSelectedItemIndex = index;
        buttons[index].Highlight(true);
    }

    private Item GetItemOnIndex(int index)
        => buttons[index].GetSlot()?.item;

    private new void Start()
    {
        base.Start();
        HighlightSlotOnIndex(_currentSelectedItemIndex);
        ToolbarManager.Instance.OnSelectedSlotIndexChanged.AddListener(HighlightSlotOnIndex);
    }
}
