using System.Collections.Generic;
using Base.Items;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    [SerializeField] public int slotsCount;

    [SerializeField] private ContainerPanel containerPanelPrefab;

    public List<ItemSlot> Slots { get; private set; }

    public ContainerPanel SpawnPanel()
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

        if (item.isStackable && existingSlot != null)
        {
            existingSlot.amount += amount;
            return;
        }

        var emptySlot = Slots.Find(s => s.item == null);

        if (emptySlot == null)
            return;

        emptySlot.item = item;
        emptySlot.amount = amount;

        Save();
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

    private void Save()
    {
        var slotSaveItems = ItemSaveHelper.ItemSlotsToSlotSaveItems(Slots);
        GameDataController.Save(slotSaveItems, DataCategory.Containers, gameObject.name);
    }

    private void Load()
    {
        var savedItems = GameDataController.Load<List<SlotSaveItem>>(DataCategory.Containers, gameObject.name);
        Slots = ItemSaveHelper.SlotSaveItemsToItemSlots(savedItems);
    }
}