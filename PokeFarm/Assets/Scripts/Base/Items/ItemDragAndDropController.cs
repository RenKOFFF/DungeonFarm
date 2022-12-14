using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    [SerializeField] private GameObject draggingItemIconPrefab;

    private GameObject _draggingItemIcon;
    private ItemSlot _draggingSlot;
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

        GameManager.Instance.inventory.Save();
        UpdateIcon();
    }

    private void InteractWithSlotLeftClick(ItemSlot clickedSlot)
    {
        if (!ItemSlot.TryMerge(_draggingSlot, clickedSlot))
            ItemSlot.Swap(_draggingSlot, clickedSlot);
    }

    private void InteractWithSlotRightClick(ItemSlot clickedSlot)
    {
        if (_draggingSlot.item == null)
        {
            var halfAmount = (int) Math.Ceiling(clickedSlot.amount / 2f);
            clickedSlot.SendAmount(_draggingSlot, halfAmount);
            return;
        }

        if (clickedSlot.item == null
            || (clickedSlot.item != null
                && clickedSlot.item.isStackable
                && clickedSlot.item == _draggingSlot.item))
        {
            _draggingSlot.SendAmount(clickedSlot);
            return;
        }

        InteractWithSlotLeftClick(clickedSlot);
    }

    private void UpdateIcon()
    {
        if (_draggingSlot.item == null)
        {
            Destroy(_draggingItemIcon);
            return;
        }

        if (_draggingItemIcon == null)
            SpawnDraggingItemIcon();

        _iconImage.sprite = _draggingSlot.item.icon;
    }

    private void Start()
    {
        _draggingSlot = new ItemSlot();
    }

    private void SpawnDraggingItemIcon()
    {
        _draggingItemIcon = SpawnManager.Instance.SpawnObjectOnCanvas(draggingItemIconPrefab);
        _iconTransform = _draggingItemIcon.GetComponent<RectTransform>();
        _iconImage = _draggingItemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if (_draggingItemIcon == null || !_draggingItemIcon.activeInHierarchy) return;

        var worldPosition = (Vector2) Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _iconTransform.position = worldPosition;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnManager.Instance.SpawnPickUpItem(worldPosition, _draggingSlot.item, _draggingSlot.amount);
            _draggingSlot.Clear();
            UpdateIcon();
        }
    }
}
