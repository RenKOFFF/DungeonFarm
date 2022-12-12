using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/GrowCycleTile")]
public class GrowCycleTile : ScriptableTileData
{
    [CanBeNull] public TileBase nextCycleTile;
}
