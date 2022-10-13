using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemContainer))]
public class ItemContainerEditor : Editor
{
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
            var random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);

            foreach (var slot in ((ItemContainer) target).slots)
            {
                slot.Clear();

                var slotIsEmpty = random.NextInt(0, 100) <= 20;

                if (slotIsEmpty)
                    continue;

                slot.item = allItems[random.NextInt(0, allItems.Length)];

                if (slot.item!.isStackable)
                    slot.amount = random.NextInt(1, 999);
            }
        }

        DrawDefaultInspector();
    }
}
