using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private ItemContainer inventory;
    [SerializeField] private List<InventoryButton> buttons;

    private void Start()
    {
        SetButtonIndexes();
    }

    private void OnEnable()
    {
        Show();
    }

    private void SetButtonIndexes()
    {
        for (var i = 0; i < inventory.slots.Count; i++)
            buttons[i].SetIndex(i);
    }

    private void Show()
    {
        for (var i = 0; i < inventory.slots.Count; i++)
        {
            var currentSlot = inventory.slots[i];

            if (currentSlot.item == null)
            {
                buttons[i].Clean();
                continue;
            }

            buttons[i].Set(currentSlot);
        }
    }
}
