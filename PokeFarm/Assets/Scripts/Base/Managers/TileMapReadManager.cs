using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool IsPlowable;

    public bool IsBreakable;
    public BreakableTile BreakableTile;
}

public class TileMapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap landscapeTilemap;
    [SerializeField] private List<TileBase> plowableTiles;

    public static TileMapReadManager Instance { get; private set; }

    private static Dictionary<TileBase, BreakableTile> _breakableTiles;
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

        _breakableTiles = Resources.LoadAll<BreakableTile>("BreakableTiles")
            .ToDictionary(bt => bt.tile);
    }

    private void Start()
    {
        _dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileBase in plowableTiles)
            _dataFromTiles.Add(tileBase, new TileData { IsPlowable = true });

        foreach (var (tileBase, breakableTile) in _breakableTiles)
            _dataFromTiles.Add(
                tileBase,
                new TileData
                {
                    IsBreakable = true,
                    BreakableTile = breakableTile
                });
    }
}
