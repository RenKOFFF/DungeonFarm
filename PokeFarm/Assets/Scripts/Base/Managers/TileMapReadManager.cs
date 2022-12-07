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
}

public class TileMapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap landscapeTilemap;
    [SerializeField] public Tilemap plantsTilemap;

    public static TileMapReadManager Instance { get; private set; }

    private static Dictionary<TileBase, TileData> _dataFromTiles;

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
        if (tileBase == null || !_dataFromTiles.ContainsKey(tileBase))
            return new TileData();

        return _dataFromTiles[tileBase];
    }

    private void Awake()
    {
        Instance = this;

        _dataFromTiles = new Dictionary<TileBase, TileData>();

        LoadScriptableTilesData<BreakableTile>(setTileData: tile => new TileData { BreakableTile = tile });
        LoadScriptableTilesData<PlantingCycleTile>(setTileData: tile => new TileData { PlantingCycleTile = tile });

        WorldTimer.AddOnDayChangedHandler(UpdatePlantingCycleTiles);
    }

    private static void LoadScriptableTilesData<T>(
        Func<T, TileData> setTileData = null,
        Action<TileData> updateTileData = null)
        where T : ScriptableTileData
    {
        var path = $"ScriptableTiles/{typeof(T)}s";
        var scriptableTilesData = Resources.LoadAll<T>(path)
            .ToDictionary(t => t.tile);

        if (scriptableTilesData.Count == 0)
        {
            Debug.LogError($"Не найдено ни одного [{typeof(T)}] в директории [Resources/{path}]." +
                           " Проверьте правильность названия директории.");
            return;
        }

        foreach (var (tileBase, scriptableTileData) in scriptableTilesData)
        {
            if (_dataFromTiles.ContainsKey(tileBase))
            {
                UpdateTileData(updateTileData, scriptableTileData, tileBase);
                continue;
            }

            SetTileData(setTileData, scriptableTileData, tileBase);
        }
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

        _dataFromTiles[tileBase] = setTileData.Invoke(scriptableTileData);
    }

    private static void UpdateTileData<T>(Action<TileData> updateTileData, T scriptableTileData, TileBase tileBase)
        where T : ScriptableTileData
    {
        if (updateTileData == null)
        {
            Debug.LogError($"Не удалось обновить данные тайла [{scriptableTileData.name}]." +
                           $" Явно укажите параметр [{nameof(updateTileData)}].");
            return;
        }

        updateTileData.Invoke(_dataFromTiles[tileBase]);
    }

    private void UpdatePlantingCycleTiles()
    {
        var cellBounds = backgroundTilemap.cellBounds;

        for (var x = cellBounds.xMin; x < cellBounds.xMax; x++)
        {
            for (var y = cellBounds.yMin; y < cellBounds.yMax; y++)
            {
                var gridPosition = new Vector3Int(x, y);
                var tileData = GetBackgroundTileDataByGridPosition(gridPosition);

                if (!tileData.IsPlantingCycleTile)
                    continue;

                var plantingCycleTile = tileData.PlantingCycleTile;

                if (plantingCycleTile.previousCycleTile != null)
                    backgroundTilemap.SetTile(gridPosition, plantingCycleTile.previousCycleTile);
            }
        }
    }
}
