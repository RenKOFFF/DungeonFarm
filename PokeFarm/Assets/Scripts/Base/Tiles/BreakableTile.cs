using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/BreakableTile")]
public class BreakableTile : ScriptableObject
{
    public TileBase tile;
    public Item drops;
    public int amount;
    [FormerlySerializedAs("breaksBy")] public Item breaksByTool;
}
