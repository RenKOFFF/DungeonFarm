using UnityEngine.EventSystems;

public class InventoryPanel : ItemPanel
{
    public override void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        GameManager.Instance.dragAndDropController.OnClick(inventory.slots[id], inputButton);
        Refresh();
    }
}
