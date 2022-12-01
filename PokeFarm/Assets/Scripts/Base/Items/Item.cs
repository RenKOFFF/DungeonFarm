using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string Name;
    public Sprite icon;
    public bool isStackable;
    public ItemType type;
}
