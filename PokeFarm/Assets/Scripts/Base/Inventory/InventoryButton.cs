using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    private int index;

    public void SetIndex(int givenIndex)
        => index = givenIndex;

    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        text.gameObject.SetActive(slot.item.isStackable);

        if (slot.item.isStackable)
            text.text = slot.count.ToString();
    }

    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var inventory = GameManager.Instance.inventoryContainer;
        GameManager.Instance.dragAndDropController.OnClick(inventory.slots[index]);
        transform.parent.GetComponent<InventoryPanel>()?.Refresh();
    }
}
