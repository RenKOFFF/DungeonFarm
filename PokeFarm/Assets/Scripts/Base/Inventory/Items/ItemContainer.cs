using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    [CanBeNull] public Item item;
    public int count;

    public ItemSlot() { }

    public ItemSlot(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public ItemSlot Copy()
        => new(item, count);

    public void Paste(ItemSlot slot)
    {
        item = slot.item;
        count = slot.count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }

    public static void Swap(ItemSlot slotA, ItemSlot slotB)
    {
        var temp = slotB.Copy();
        slotB.Paste(slotA);
        slotA.Paste(temp);
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
