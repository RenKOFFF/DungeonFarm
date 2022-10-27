using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileMapReadManager : MonoBehaviour
{
    [SerializeField] public Tilemap backgroundTilemap;
    [SerializeField] public Tilemap landscapeTilemap;
    [SerializeField] private List<TileData> tileDatas;

    public static TileMapReadManager Instance;

    private Dictionary<TileBase, TileData> dataFromTiles;

    public Vector3Int GetCurrentBackgroundGridPositionByMousePosition()
        => GetBackgroundGridPosition(Input.mousePosition, true);

    private Vector3Int GetBackgroundGridPosition(Vector2 position, bool isMousePosition = false)
        => GetGridPosition(backgroundTilemap, position, isMousePosition);

    private static Vector3Int GetGridPosition(GridLayout tilemap, Vector2 position, bool isMousePosition)
    {
        Vector2 worldPosition = isMousePosition
            ? Camera.main.ScreenToWorldPoint(position)
            : position;

        return tilemap.WorldToCell(worldPosition);
    }

    private TileBase GetTileBase(Vector2 position, bool isMousePosition = false)
    {
        var gridPosition = GetBackgroundGridPosition(position, isMousePosition);
        var tile = backgroundTilemap.GetTile(gridPosition);

        return tile;
    }

    private TileData GetTileData(TileBase tileBase)
        => dataFromTiles[tileBase];

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }
}
