using System;
using System.Collections.Generic;
using Base.Items;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    [SerializeField] public int slotsCount;

    [SerializeField] private ContainerPanelSpawnable containerPanelPrefab;

    public List<ItemSlot> Slots { get; private set; }

    public ContainerPanelSpawnable SpawnPanel()
    {
        var canvas = GameObject.FindWithTag("Canvas");
        var containerPanel = Instantiate(containerPanelPrefab, canvas.transform, false);

        containerPanel.Init(this);
        containerPanel.Open();

        return containerPanel;
    }

    public void Add(Item item, int amount = 1)
    {
        var existingSlot = Slots.Find(s => s.item == item);

        if (existingSlot == null && amount <= 0)
        {
            Debug.LogError($"Попытка удалить несуществующий предмет [{item.Name}]. Количество: {amount}.");
            return;
        }

        if (existingSlot == null || !item.isStackable && amount > 0)
        {
            TryAddToEmptySlot(item, amount);
            return;
        }

        existingSlot.amount += amount;

        if (existingSlot.amount <= 0)
            existingSlot.Clear();

        SaveAndRefresh();
    }

    private bool TryAddToEmptySlot(Item item, int amount = 1)
    {
        var emptySlot = Slots.Find(s => s.item == null);

        if (emptySlot == null)
            return false;

        emptySlot.item = item;
        emptySlot.amount = amount;

        SaveAndRefresh();
        return true;
    }

    public void Remove(Item item, int amount = 1)
        => Add(item, -amount);

    private void SaveAndRefresh()
    {
        InventoryManager.Instance.Refresh();
        Save();
    }

    public void Save()
    {
        var slotSaveItems = ItemSaveHelper.ItemSlotsToSlotSaveItems(Slots);
        GameDataController.Save(slotSaveItems, DataCategory.Containers, gameObject.name);
    }

    private void Load()
    {
        var savedItems = GameDataController.LoadWithInitializationIfEmpty<List<SlotSaveItem>>(
            DataCategory.Containers,
            gameObject.name);

        Slots = ItemSaveHelper.SlotSaveItemsToItemSlots(savedItems);
    }

    private void Awake()
    {
        OnEnable();
        
    }

    private void Start()
    {
        InventoryManager.Instance?.Refresh();
    }

    private void OnEnable()
    {
        Load();

        if (Slots != null && Slots.Count == slotsCount)
            return;

        Slots = new List<ItemSlot>();

        for (var i = 0; i < slotsCount; i++)
            Slots.Add(new ItemSlot());

        Save();
    }
}
