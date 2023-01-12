using JetBrains.Annotations;
using UnityEngine;

public enum ItemType
{
    NotAssigned,
    Tool,
    Food,
    Building,
    Seed,
}

[CreateAssetMenu(menuName = "Data/Item")]
public class Item : ScriptableObject
{
    public int id;

    public string Name;

    public Sprite icon;

    public bool isStackable;

    public ItemType type;

    [CanBeNull, Header("Plant seeds")] public GrowCycleTile growCycleStartTile;

    [CanBeNull, Header("Buildings")] public GameObject BuildingPrefab;
}
