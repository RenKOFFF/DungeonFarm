using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    [FormerlySerializedAs("stackable")] public bool isStackable;
    public Sprite icon;
}
