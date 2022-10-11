using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private ItemContainer inventory;
    [SerializeField] private List<InventoryButton> buttons;

    public void Refresh()
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

    private void SetButtonIndexes()
    {
        for (var i = 0; i < inventory.slots.Count; i++)
            buttons[i].SetIndex(i);
    }

    private void Start()
    {
        SetButtonIndexes();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Refresh();
    }
}