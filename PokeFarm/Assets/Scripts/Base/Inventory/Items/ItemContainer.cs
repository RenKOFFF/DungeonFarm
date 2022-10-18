using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    [SerializeField] [CanBeNull] public Item item;
    [SerializeField] public int amount;

    public ItemSlot() { }

    public ItemSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public static void Swap(ItemSlot slotA, ItemSlot slotB)
    {
        var temp = slotB.Copy();
        slotB.Paste(slotA);
        slotA.Paste(temp);
    }

    public static bool TryMerge(ItemSlot from, ItemSlot to)
    {
        if (from.item == null
            || !from.item.isStackable
            || from.item != to.item)
        {
            return false;
        }

        from.SendAll(to);

        return true;
    }

    public ItemSlot Copy()
        => new(item, amount);

    public void Paste(ItemSlot slot)
    {
        item = slot.item;
        amount = slot.amount;
    }

    public void Paste(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }

    public void SendAmount(ItemSlot destination, int amount = 1)
    {
        if (amount > this.amount)
        {
            Debug.LogError($"Попытка передать в слот количество предметов [{amount}], превышающее текущее [{this.amount}].");
            return;
        }

        if (destination.item == null)
            destination.item = item;

        destination.AddAmount(amount);
        AddAmount(-amount);

        if (this.amount == 0)
            Clear();
    }

    public void SendAll(ItemSlot destination)
    {
        SendAmount(destination, amount);
    }

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
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
