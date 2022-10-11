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
                slot.CLear();
        }

        DrawDefaultInspector();
    }
}
