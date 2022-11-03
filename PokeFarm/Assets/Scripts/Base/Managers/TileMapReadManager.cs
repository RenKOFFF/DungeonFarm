using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool plowable;
    public bool toolInteractable;
}

public class TileMapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap landscapeTilemap;
    [SerializeField] private List<TileBase> plowableTiles;
    [SerializeField] private List<TileBase> toolInteractableTiles;

    public static TileMapReadManager Instance { get; private set; }

    private Dictionary<TileBase, TileData> dataFromTiles;

    public static Vector3Int GetGridPosition(GridLayout tilemap, Vector2 position, bool isMousePosition)
    {
        Vector2 worldPosition = isMousePosition
            ? Camera.main.ScreenToWorldPoint(position)
            : position;

        return tilemap.WorldToCell(worldPosition);
    }

    public Vector3Int GetCurrentBackgroundGridPositionByMousePosition()
        => GetGridPosition(backgroundTilemap, Input.mousePosition, true);

    public TileData GetLandscapeTileDataByMousePosition()
        => GetTileData(GetTileBase(landscapeTilemap, Input.mousePosition, true));

    private TileBase GetTileBase(Tilemap tilemap, Vector2 position, bool isMousePosition = false)
    {
        var gridPosition = GetGridPosition(tilemap, position, isMousePosition);
        var tile = tilemap.GetTile(gridPosition);

        return tile;
    }

    private TileData GetTileData(TileBase tileBase)
    {
        if (tileBase == null || !dataFromTiles.ContainsKey(tileBase))
            return new TileData();

        return dataFromTiles[tileBase];
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileBase in plowableTiles)
            dataFromTiles.Add(tileBase, new TileData { plowable = true });

        foreach (var tileBase in toolInteractableTiles)
            dataFromTiles.Add(tileBase, new TileData { toolInteractable = true });
    }
}
