using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReader : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private TileBase GetTileBase(Vector2 position, bool isMousePosition = false)
    {
        var gridPosition = GetGridPosition(position, isMousePosition);
        var tile = tilemap.GetTile(gridPosition);

        return tile;
    }

    private Vector3Int GetGridPosition(Vector2 position, bool isMousePosition)
    {
        Vector2 worldPosition = isMousePosition
            ? Camera.main.ScreenToWorldPoint(position)
            : position;

        return tilemap.WorldToCell(worldPosition);
    }

    private TileData GetTileData(TileBase tileBase)
        => dataFromTiles[tileBase];

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
