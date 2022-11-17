using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image highlightImage;

    private ItemSlot Slot { get; set; }
    private int Index { get; set; }

    public void SetIndex(int givenIndex)
        => Index = givenIndex;

    public void Set(ItemSlot slot)
    {
        Slot = slot;
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
        var itemPanel = GetComponentInParent<ContainerPanel>();
        itemPanel.OnClick(Index, eventData.button);
    }

    public void Highlight(bool makeActive)
    {
        highlightImage.gameObject.SetActive(makeActive);
    }

    public ItemSlot GetSlot()
        => Slot;
}
