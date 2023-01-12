using Base.Monsters;
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

    [CanBeNull, Header("Plants")] public GrowCycleTile growCycleStartTile;
    [SerializeField] private MonsterStats _addingStats;

    public MonsterStats AddingStats => _addingStats;

    [CanBeNull, Header("Buildings")] public GameObject BuildingPrefab;
}
