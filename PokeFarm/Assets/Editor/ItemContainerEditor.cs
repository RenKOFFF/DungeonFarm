using System;
using Base.Items;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainerOld))]
public class ItemContainerEditor : Editor
{
    private const string SavedInventoryFileName = "MainCharacter";

    private Unity.Mathematics.Random _random;

    private void Awake()
    {
        _random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Clear container"))
        {
            foreach (var slot in ((ItemContainerOld) target).slots)
                slot.Clear();

            Save();
        }

        if (GUILayout.Button("Set random"))
        {
            var allItems = Resources.LoadAll<Item>("Items");

            foreach (var slot in ((ItemContainerOld) target).slots)
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

        DrawDefaultInspector();
    }

    private void Save()
    {
        var slotSaveItems = ItemSaveHelper.ItemSlotsToSlotSaveItems(((ItemContainerOld) target).slots);
        GameDataController.Save(slotSaveItems, DataCategory.Containers, SavedInventoryFileName);
    }
}
