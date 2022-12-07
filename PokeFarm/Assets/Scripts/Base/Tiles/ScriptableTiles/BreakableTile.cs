using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BreakableTile")]
public class BreakableTile : ScriptableTileData
{
    public Item dropsItem;
    public int dropAmount;

    [CanBeNull] public Item breaksByTool;
    public bool breaksByAny;
}
