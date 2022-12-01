using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolsController : MonoBehaviour
{
    private static void UseTool(Item tool)
    {
        var currentGridPosition = MarkerManager.Instance.markedCellPosition;

        var backgroundTilemap = TileMapReadManager.Instance.backgroundTilemap;
        var landscapeTilemap = TileMapReadManager.Instance.landscapeTilemap;

        var backgroundTileData = TileMapReadManager.Instance.GetBackgroundTileDataByGridPosition(currentGridPosition);
        var landscapeTileData = TileMapReadManager.Instance.GetLandscapeTileDataByGridPosition(currentGridPosition);

        if (landscapeTileData.IsBreakable)
        {
            UseToolOnBreakableTile(
                tool,
                landscapeTileData,
                landscapeTilemap,
                currentGridPosition);
            return;
        }

        if (backgroundTileData.IsPlowable)
        {
            if (tool != GameDataController.AllItems["Hoe"])
                return;

            backgroundTilemap.SetTile(
                currentGridPosition,
                null);

            return;
        }
    }

    private static void UseToolOnBreakableTile(
        Item tool,
        TileData tileData,
        Tilemap landscapeTilemap,
        Vector3Int currentGridPosition)
    {
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

        var currentItemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;

        if (currentItemOnTheHand == null || currentItemOnTheHand.type != ItemType.Tool)
            return;

        UseTool(currentItemOnTheHand);
    }
}
