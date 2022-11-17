using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/BreakableTile")]
public class BreakableTile : ScriptableObject
{
    public TileBase tile;
    public Item dropsItem;
    public int dropAmount;
    public Item breaksByTool;
}
