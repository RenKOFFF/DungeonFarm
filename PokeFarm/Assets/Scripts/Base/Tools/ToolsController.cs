using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ToolsController : MonoBehaviour
{
    public static UnityEvent<PlantingCycleTile, Vector3Int> OnGardenBedDigUpEvent = new();

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

            TileMapReadManager.SaveTileMap(landscapeTilemap);
            return;
        }

        if (backgroundTileData.IsPlantingCycleTile)
        {
            UseToolOnPlantingCycleTile(tool, backgroundTileData, backgroundTilemap, currentGridPosition);

            TileMapReadManager.SaveTileMap(backgroundTilemap);
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

    private static void UseToolOnPlantingCycleTile(
        Item tool,
        TileData backgroundTileData,
        Tilemap backgroundTilemap,
        Vector3Int currentGridPosition)
    {
        var plantingCycleTile = backgroundTileData.PlantingCycleTile;

        if (tool != plantingCycleTile.interactsWithTool)
            return;

        backgroundTilemap.SetTile(
            currentGridPosition,
            plantingCycleTile.nextCycleTile);

        //поставили грядку или полили
        OnGardenBedDigUpEvent.Invoke(plantingCycleTile, currentGridPosition);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        //TODO temp solve
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainScene"))
            return;

        var currentItemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;

        if (currentItemOnTheHand == null || currentItemOnTheHand.type != ItemType.Tool)
            return;

        UseTool(currentItemOnTheHand);
    }
}
