using System;
using System.Collections.Generic;
using Base.Items;
using DeveloperTools.Scripts;
using UnityEditor;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[CustomEditor(typeof(InventoryEditTool))]
public class ItemContainerEditor : Editor
{
    private const string SavedInventoryFileName = "MainCharacter";

    private Random _random;
    private InventoryEditTool _currentInventory;

    private void Awake()
    {
        _random = new Random((uint) DateTime.Now.Millisecond);
        _currentInventory = (InventoryEditTool) target;
        Load();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Clear"))
        {
            foreach (var slot in _currentInventory.slots)
                slot.Clear();

            Save();
        }

        if (GUILayout.Button("Set random"))
        {
            var allItems = Resources.LoadAll<Item>("Items");

            foreach (var slot in _currentInventory.slots)
            {
                slot.Clear();

                var isSlotEmpty = _random.NextInt(1, 100) <= 20;

                if (isSlotEmpty)
                    continue;

                slot.item = allItems[_random.NextInt(0, allItems.Length)];
                slot.amount = slot.item.isStackable ? _random.NextInt(1, 999) : 1;
            }

            Save();
        }

        if (GUILayout.Button("Save"))
        {
            Save();
        }

        DrawDefaultInspector();
    }

    private void Save()
    {
        var slotSaveItems = ItemSaveHelper.ItemSlotsToSlotSaveItems(_currentInventory.slots);
        GameDataController.Save(slotSaveItems, DataCategory.Containers, SavedInventoryFileName);
    }

    private void Load()
    {
        var savedItems = GameDataController.LoadWithInitializationIfEmpty<List<SlotSaveItem>>(DataCategory.Containers, SavedInventoryFileName);
        _currentInventory.slots = ItemSaveHelper.SlotSaveItemsToItemSlots(savedItems);
    }
}
