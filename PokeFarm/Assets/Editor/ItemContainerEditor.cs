using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainer))]
public class ItemContainerEditor : Editor
{
    private Unity.Mathematics.Random random;

    private void Awake()
    {
        random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Clear container"))
        {
            foreach (var slot in ((ItemContainer) target).slots)
                slot.Clear();
        }

        if (GUILayout.Button("Set random"))
        {
            var allItems = Resources.LoadAll<Item>("Items");

            foreach (var slot in ((ItemContainer) target).slots)
            {
                slot.Clear();

                var isSlotEmpty = random.NextInt(1, 100) <= 20;

                if (isSlotEmpty)
                    continue;

                slot.item = allItems[random.NextInt(0, allItems.Length)];
                slot.amount = slot.item.isStackable ? random.NextInt(1, 999) : 1;
            }
        }

        DrawDefaultInspector();
    }
}
