using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] private ItemSlot draggingSlot;
    [SerializeField] private GameObject itemIcon;
    private RectTransform iconTransform;
    private Image iconImage;

    public void OnClick(ItemSlot otherSlot)
    {
        if (draggingSlot.item == null)
        {
            draggingSlot.Paste(otherSlot);
            otherSlot.Clear();
            UpdateIcon();
            return;
        }

        ItemSlot.Swap(draggingSlot, otherSlot);
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        itemIcon.SetActive(draggingSlot.item != null);

        if (draggingSlot.item != null)
            iconImage.sprite = draggingSlot.item.icon;
    }

    private void Start()
    {
        draggingSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        iconImage = itemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if (!itemIcon.activeInHierarchy) return;

        iconTransform.position = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // TODO нужен обработчик клика вне панели
        }
    }
}
