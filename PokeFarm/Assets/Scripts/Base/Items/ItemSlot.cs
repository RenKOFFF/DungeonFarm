﻿using System;
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
