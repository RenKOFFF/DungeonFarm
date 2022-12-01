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

    public override void Refresh()
    {
        if (containerController.Slots == null)
            return;

        for (var i = 0; i < containerController.slotsCount && i < inventoryButtons.Count; i++)
        {
            var currentSlot = containerController.Slots[i];

            if (currentSlot.item == null)
            {
                if (i == _currentSelectedItemIndex)
                {
                    ToolbarManager.Instance?.RefreshItemOnTheHand();
                }
                inventoryButtons[i].Clean();
                continue;
            }

            inventoryButtons[i].Set(currentSlot);
        }

        containerController.Save();
    }
}
