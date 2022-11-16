using UnityEngine;

public enum ItemType
{
    NotAssigned,
    Tool,
}

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite icon;
    public bool isStackable;
    public ItemType type;
    public bool isFood;
}
