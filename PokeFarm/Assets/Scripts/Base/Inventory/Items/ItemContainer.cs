using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void PasteValues(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void CLear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int count = 1)
    {
        var existingSlot = slots.Find(s => s.item == item);
        if (item.isStackable && existingSlot != null)
        {
            existingSlot.count++;
            return;
        }

        var emptySlot = slots.Find(s => s.item == null);
        if (emptySlot == null) return;

        emptySlot.item = item;
        emptySlot.count = count;
    }
}
