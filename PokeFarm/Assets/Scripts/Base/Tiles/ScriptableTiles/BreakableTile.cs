using UnityEngine;

[CreateAssetMenu(menuName = "Data/BreakableTile")]
public class BreakableTile : ScriptableTileData
{
    public Item dropsItem;
    public int dropAmount;
    public Item breaksByTool;
}
