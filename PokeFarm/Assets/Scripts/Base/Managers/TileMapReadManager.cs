using System;
using System.Collections.Generic;
using System.Linq;
using Base.Time;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool IsBreakable => BreakableTile != null;
    [CanBeNull] public BreakableTile BreakableTile;

    public bool IsPlantingCycleTile => PlantingCycleTile != null;
    [CanBeNull] public PlantingCycleTile PlantingCycleTile;

    public bool IsGrowCycleTile => GrowCycleTile != null;
    [CanBeNull] public GrowCycleTile GrowCycleTile;
}

public class TileMapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap landscapeTilemap;
    [SerializeField] public Tilemap plantsTilemap;

    public static TileMapReadManager Instance { get; private set; }

    private static Dictionary<TileBase, TileData> DataFromTiles { get; set; } = new();
    private static Dictionary<string, TileBase> TileNameToTile { get; } = new();

    private const string CoordinatesSaveItemSeparator = ", ";

    public static Vector3Int GetGridPosition(GridLayout tilemap, Vector2 position, bool isMousePosition)
    {
        Vector2 worldPosition = isMousePosition
            ? Camera.main.ScreenToWorldPoint(position)
            : position;

        return tilemap.WorldToCell(worldPosition);
    }

    public static Vector3 GetCellCenterWorldPosition(Tilemap tilemap, Vector3Int gridPosition)
        => tilemap.CellToWorld(gridPosition) + tilemap.layoutGrid.cellSize / 2;

    public static TileBase GetTileBase(Tilemap tilemap, Vector3Int gridPosition)
        => tilemap.GetTile(gridPosition);

    public Vector3Int GetCurrentBackgroundGridPositionByMousePosition()
        => GetGridPosition(backgroundTilemap, Input.mousePosition, true);

    public TileData GetLandscapeTileDataByMousePosition()
        => GetTileData(GetTileBase(landscapeTilemap, Input.mousePosition, true));

    public TileData GetBackgroundTileDataByGridPosition(Vector3Int gridPosition)
        => GetTileData(GetTileBase(backgroundTilemap, gridPosition));

    public TileData GetLandscapeTileDataByGridPosition(Vector3Int gridPosition)
        => GetTileData(GetTileBase(landscapeTilemap, gridPosition));

    public TileData GetPlantsTileDataByGridPosition(Vector3Int gridPosition)
        => GetTileData(GetTileBase(plantsTilemap, gridPosition));

    private static TileBase GetTileBase(Tilemap tilemap, Vector2 position, bool isMousePosition = false)
        => GetTileBase(tilemap, GetGridPosition(tilemap, position, isMousePosition));

    private static TileData GetTileData(TileBase tileBase)
    {
        if (tileBase == null || !DataFromTiles.ContainsKey(tileBase))
            return new TileData();

        return DataFromTiles[tileBase];
    }

    private static void LoadScriptableTilesData<T>(
        Func<T, TileData> setTileData = null,
        Action<TileData, T> updateTileData = null)
        where T : ScriptableTileData
    {
        var path = $"ScriptableTiles/{typeof(T)}s";
        var tileToScriptableTileData = Resources.LoadAll<T>(path)
            .ToDictionary(t => t.tile);

        if (tileToScriptableTileData.Count == 0)
        {
            Debug.LogError($"Не найдено ни одного [{typeof(T)}] в директории [Resources/{path}]." +
                           " Проверьте правильность названия директории.");
            return;
        }

        foreach (var (tileBase, scriptableTileData) in tileToScriptableTileData)
        {
            if (DataFromTiles.ContainsKey(tileBase))
            {
                UpdateTileData(updateTileData, scriptableTileData, tileBase);
                continue;
            }

            SetTileData(setTileData, scriptableTileData, tileBase);
        }

        AddScriptableTilesToAllTiles(tileToScriptableTileData);
    }

    private static void AddScriptableTilesToAllTiles<T>(Dictionary<TileBase, T> tileToScriptableTileData)
        where T : ScriptableTileData
    {
        foreach (var (tile, _) in tileToScriptableTileData)
            TileNameToTile[tile.name] = tile;
    }

    private static void SetTileData<T>(Func<T, TileData> setTileData, T scriptableTileData, TileBase tileBase)
        where T : ScriptableTileData
    {
        if (setTileData == null)
        {
            Debug.LogError($"Не удалось загрузить данные тайла [{scriptableTileData.name}]." +
                           $" Явно укажите параметр [{nameof(setTileData)}].");
            return;
        }

        DataFromTiles[tileBase] = setTileData.Invoke(scriptableTileData);
    }

    private static void UpdateTileData<T>(Action<TileData, T> updateTileData, T scriptableTileData, TileBase tileBase)
        where T : ScriptableTileData
    {
        if (updateTileData == null)
        {
            Debug.LogError($"Не удалось обновить данные тайла [{scriptableTileData.name}]." +
                           $" Явно укажите параметр [{nameof(updateTileData)}].");
            return;
        }

        updateTileData.Invoke(DataFromTiles[tileBase], scriptableTileData);
    }

    private static void UpdateTilemap(Tilemap tilemap, Action<Tilemap, Vector3Int> updateTileByCoordinates)
    {
        var cellBounds = tilemap.cellBounds;
        var tilePositionToTile = new Dictionary<string, string>();

        for (var x = cellBounds.xMin; x < cellBounds.xMax; x++)
        for (var y = cellBounds.yMin; y < cellBounds.yMax; y++)
        {
            var gridPosition = new Vector3Int(x, y);
            updateTileByCoordinates(tilemap, gridPosition);

            var tile = tilemap.GetTile(gridPosition);

            if (tile != null)
                tilePositionToTile.Add(GetTileCoordinatesSaveItem(x, y), tile.name);
        }

        SaveTileMap(tilemap, tilePositionToTile);
    }

    private void UpdatePlantingCycleTiles()
    {
        UpdateTilemap(
            backgroundTilemap,
            (tilemap, gridPosition) =>
            {
                var tileData = GetBackgroundTileDataByGridPosition(gridPosition);

                if (!tileData.IsPlantingCycleTile)
                    return;

                var plantingCycleTile = tileData.PlantingCycleTile;

                if (plantingCycleTile.previousCycleTile != null)
                    tilemap.SetTile(gridPosition, plantingCycleTile.previousCycleTile);
            });
    }

    private void UpdateGrowCycleTiles()
    {
        UpdateTilemap(
            plantsTilemap,
            (tilemap, gridPosition) =>
            {
                var plantsTileData = GetPlantsTileDataByGridPosition(gridPosition);

                if (!plantsTileData.IsGrowCycleTile)
                    return;

                var backgroundTileData = GetBackgroundTileDataByGridPosition(gridPosition);
                var growCycleTile = plantsTileData.GrowCycleTile;

                if (growCycleTile.nextCycleTile != null)
                    tilemap.SetTile(gridPosition, growCycleTile.nextCycleTile);

                if (!backgroundTileData.IsPlantingCycleTile || !backgroundTileData.PlantingCycleTile.availableForPlant)
                    tilemap.SetTile(gridPosition, null);
            });
    }

    public static void SaveTileMap(Tilemap tilemap)
    {
        var cellBounds = tilemap.cellBounds;
        var tilePositionToTile = new Dictionary<string, string>();

        for (var x = cellBounds.xMin; x < cellBounds.xMax; x++)
        for (var y = cellBounds.yMin; y < cellBounds.yMax; y++)
        {
            var gridPosition = new Vector3Int(x, y);
            var tile = tilemap.GetTile(gridPosition);

            tilePositionToTile.Add(GetTileCoordinatesSaveItem(x, y), tile?.name);
        }

        Debug.Log(tilemap.name);
        GameDataController.Save(tilePositionToTile, DataCategory.Tilemaps, tilemap.name);
    }

    private static void SaveTileMap(Tilemap tilemap, Dictionary<string, string> tilePositionToTile)
    {
        GameDataController.Save(tilePositionToTile, DataCategory.Tilemaps, tilemap.name);
    }

    private static string GetTileCoordinatesSaveItem(int x, int y)
        => $"{x}{CoordinatesSaveItemSeparator}{y}";

    private static void LoadTileMap(Tilemap tilemap)
    {
        var tilePositionToTile = GameDataController.LoadWithInitializationIfEmpty<Dictionary<string, string>>(
            DataCategory.Tilemaps,
            tilemap.name);

        foreach (var (tileCoordinatesString, tileName) in tilePositionToTile)
        {
            if (tileName != null && !TileNameToTile.ContainsKey(tileName))
                continue;

            TileBase tile = null;

            if (tileName != null)
                tile = TileNameToTile[tileName];

            var tileGridPosition = TryParseTileGridPosition(tileCoordinatesString);

            if (tileGridPosition == null)
                continue;

            tilemap.SetTile(tileGridPosition.Value, tile);
        }
    }

    private static Vector3Int? TryParseTileGridPosition(string tileCoordinatesString)
    {
        var splitData = tileCoordinatesString.Split(CoordinatesSaveItemSeparator);

        if (splitData.Length != 2)
            return null;

        if (!int.TryParse(splitData[0], out var x) || !int.TryParse(splitData[0], out var y))
            return null;

        return new Vector3Int(x, y);
    }

    private static void LoadScriptableTiles()
    {
        LoadScriptableTilesData<BreakableTile>(setTileData: tile => new TileData { BreakableTile = tile });

        LoadScriptableTilesData<PlantingCycleTile>(
            setTileData: tile => new TileData { PlantingCycleTile = tile },
            updateTileData: (tileData, tile) => tileData.PlantingCycleTile = tile);

        LoadScriptableTilesData<GrowCycleTile>(
            setTileData: tile => new TileData { GrowCycleTile = tile },
            updateTileData: (tileData, tile) => tileData.GrowCycleTile = tile);
    }

    private void LoadTilemaps()
    {
        LoadTileMap(backgroundTilemap);
        LoadTileMap(landscapeTilemap);
        LoadTileMap(plantsTilemap);
    }

    private void AddOnDayChangedHandlers()
    {
        WorldTimer.AddOnDayChangedHandler(UpdatePlantingCycleTiles);
        WorldTimer.AddOnDayChangedHandler(UpdateGrowCycleTiles);
    }

    private void Awake()
    {
        Instance = this;

        DataFromTiles = new Dictionary<TileBase, TileData>();

        LoadScriptableTiles();
        LoadTilemaps();
        AddOnDayChangedHandlers();
    }
}
