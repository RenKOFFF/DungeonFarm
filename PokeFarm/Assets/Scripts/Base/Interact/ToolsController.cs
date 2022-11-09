using UnityEngine;

public class ToolsController : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var currentItemOnTheHand = HandBank.instance.itemOnTheHand;

        if (currentItemOnTheHand == null || currentItemOnTheHand.type != ItemType.Tool)
            return;

        UseTool(currentItemOnTheHand);
    }

    private static void UseTool(Item tool)
    {
        var landscapeTilemap = TileMapReadManager.Instance.landscapeTilemap;
        var currentGridPosition = MarkerManager.Instance.markedCellPosition;
        var tileData = TileMapReadManager.Instance.GetLandscapeTileDataByGridPosition(currentGridPosition);

        if (!tileData.IsBreakable)
            return;

        var breakableTile = tileData.BreakableTile;

        if (tool != breakableTile.breaksByTool)
            return;

        landscapeTilemap.SetTile(
            currentGridPosition,
            null);
    }
}
