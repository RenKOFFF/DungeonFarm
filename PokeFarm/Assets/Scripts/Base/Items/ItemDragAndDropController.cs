using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] private ItemSlot draggingSlot;
    [SerializeField] private GameObject itemIcon;

    private RectTransform iconTransform;
    private Image iconImage;

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

        var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        iconTransform.position = worldPosition;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnManager.Instance.SpawnPickUpItem(worldPosition, draggingSlot.item, draggingSlot.amount);
            draggingSlot.Clear();
            UpdateIcon();
        }
    }
}
