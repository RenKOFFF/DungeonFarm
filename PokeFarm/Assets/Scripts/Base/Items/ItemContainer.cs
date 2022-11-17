using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] public int slotsCount;

    [SerializeField] private ContainerPanel containerPanelPrefab;

    public List<ItemSlot> Slots { get; private set; }

    public void SpawnPanel()
    {
        var canvas = GameObject.FindWithTag("Canvas");
        var containerPanel = Instantiate(containerPanelPrefab, canvas.transform, false);

        containerPanel.Init(this);

        containerPanel.gameObject.SetActive(true);
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
        GameDataController.Save(Slots, DataCategory.Containers, gameObject.name);
    }

    private void Load()
    {
        Slots = GameDataController.Load<List<ItemSlot>>(DataCategory.Containers, gameObject.name);
    }
}
