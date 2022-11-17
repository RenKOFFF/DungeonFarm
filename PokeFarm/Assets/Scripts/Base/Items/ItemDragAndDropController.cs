using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] private ItemSlot draggingSlot;
    [SerializeField] private GameObject itemIcon;

    private RectTransform _iconTransform;
    private Image _iconImage;

    public void OnClick(ItemSlot clickedSlot, PointerEventData.InputButton inputButton)
    {
        switch (inputButton)
        {
            case PointerEventData.InputButton.Left:
                InteractWithSlotLeftClick(clickedSlot);
                break;
            case PointerEventData.InputButton.Right:
                InteractWithSlotRightClick(clickedSlot);
                break;
            default:
                return;
        }

        UpdateIcon();
    }

    private void InteractWithSlotLeftClick(ItemSlot clickedSlot)
    {
        if (!ItemSlot.TryMerge(draggingSlot, clickedSlot))
            ItemSlot.Swap(draggingSlot, clickedSlot);
    }

    private void InteractWithSlotRightClick(ItemSlot clickedSlot)
    {
        if (draggingSlot.item == null)
        {
            var halfAmount = (int) Math.Ceiling(clickedSlot.amount / 2f);
            clickedSlot.SendAmount(draggingSlot, halfAmount);
            return;
        }

        if (clickedSlot.item == null
            || (clickedSlot.item != null
                && clickedSlot.item.isStackable
                && clickedSlot.item == draggingSlot.item))
        {
            draggingSlot.SendAmount(clickedSlot);
            return;
        }

        InteractWithSlotLeftClick(clickedSlot);
    }

    private void UpdateIcon()
    {
        itemIcon.SetActive(draggingSlot.item != null);

        if (draggingSlot.item != null)
            _iconImage.sprite = draggingSlot.item.icon;
    }

    private void Start()
    {
        draggingSlot = new ItemSlot();
        _iconTransform = itemIcon.GetComponent<RectTransform>();
        _iconImage = itemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if (!itemIcon.activeInHierarchy) return;

        var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _iconTransform.position = worldPosition;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnManager.Instance.SpawnPickUpItem(worldPosition, draggingSlot.item, draggingSlot.amount);
            draggingSlot.Clear();
            UpdateIcon();
        }
    }
}
