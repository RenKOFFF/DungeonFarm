using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainerOld : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int amount = 1)
    {
        var existingSlot = slots.Find(s => s.item == item);

        if (item.isStackable && existingSlot != null)
        {
            existingSlot.amount += amount;
            return;
        }

        var emptySlot = slots.Find(s => s.item == null);
        if (emptySlot == null) return;

        emptySlot.item = item;
        emptySlot.amount = amount;
    }
}
