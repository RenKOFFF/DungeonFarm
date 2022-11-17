using UnityEngine.EventSystems;

public class InventoryPanel : ContainerPanel
{
    public override void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        GameManager.Instance.dragAndDropController.OnClick(itemContainer.Slots[id], inputButton);
        Refresh();
    }
}
