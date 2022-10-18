using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image hightlightImage;

    private ItemSlot slot { get; set; }

    private int index;

    public void SetIndex(int givenIndex)
        => index = givenIndex;

    public void Set(ItemSlot slot)
    {
        this.slot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        text.gameObject.SetActive(slot.item.isStackable);

        if (slot.item.isStackable)
            text.text = slot.amount.ToString();
    }

    public void Clean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var itemPanel = GetComponentInParent<ItemPanel>();
        itemPanel.OnClick(index, eventData.button);
    }

    public void Highlight(bool makeActive)
    {
        hightlightImage.gameObject.SetActive(makeActive);
    }

    public ItemSlot GetSlot()
    {
        return slot;
    }
}
