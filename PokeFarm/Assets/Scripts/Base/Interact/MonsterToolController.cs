using UnityEngine;

public class MonsterToolController : MonoBehaviour
{
    public static void UseTool(Item tool, Monster monster, Vector3Int currentGridPosition)
    {
        var landscapeTilemap = TileMapReadManager.Instance.landscapeTilemap;
        var tileData = TileMapReadManager.Instance.GetLandscapeTileDataByGridPosition(currentGridPosition);

        if (!tileData.IsBreakable)
            return;

        var breakableTile = tileData.BreakableTile;

        if (tool != breakableTile.breaksByTool)
            return;

        landscapeTilemap.SetTile(
            currentGridPosition,
            null);

        monster.Inventory.AddItem(breakableTile.dropsItem/*,
            breakableTile.dropAmount*/);
    }
}
