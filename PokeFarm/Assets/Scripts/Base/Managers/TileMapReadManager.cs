using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<TileBase> plowableTiles;

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

        LoadScriptableTilesData<BreakableTile>(tile => new TileData { BreakableTile = tile });
        LoadScriptableTilesData<PlantingCycleTile>(tile => new TileData { PlantingCycleTile = tile });
    }

    private static void LoadScriptableTilesData<T>(Func<T, TileData> setTileData)
        where T : ScriptableTileData
    {
        var scriptableTilesData = Resources.LoadAll<T>($"{typeof(T)}s")
            .ToDictionary(t => t.tile);

        foreach (var (tileBase, scriptableTileData) in scriptableTilesData)
            _dataFromTiles[tileBase] = setTileData.Invoke(scriptableTileData);
    }
}
