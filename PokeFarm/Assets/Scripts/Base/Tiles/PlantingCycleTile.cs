using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/PlantingCycleTile")]
public class PlantingCycleTile : ScriptableObject
{
    public TileBase tile;

    [CanBeNull] public TileBase previousCycleTile;
    [CanBeNull] public TileBase nextCycleTile;

    [CanBeNull] public Item interactsWithTool;
}
