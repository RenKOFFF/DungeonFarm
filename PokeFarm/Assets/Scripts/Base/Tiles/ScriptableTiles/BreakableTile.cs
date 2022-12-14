using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/BreakableTile")]
public class BreakableTile : ScriptableTileData
{
    public Item dropsItem;
    public int dropAmount;

    [CanBeNull] public Item breaksByTool;
    public bool breaksByAny;

    [CanBeNull] public TileBase backgroundTileToSetAfterDestruction;
}
