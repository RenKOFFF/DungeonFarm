using UnityEngine;

public class ToolsController : MonoBehaviour
{
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

        SpawnManager.Instance.SpawnPickUpItemsInArea(
            TileMapReadManager.GetCellCenterWorldPosition(landscapeTilemap, currentGridPosition),
            breakableTile.dropsItem,
            breakableTile.dropAmount);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var currentItemOnTheHand = ToolbarController.Instance.ItemOnTheHand;

        if (currentItemOnTheHand == null || currentItemOnTheHand.type != ItemType.Tool)
            return;

        UseTool(currentItemOnTheHand);
    }
}
