using System;
using System.Collections.Generic;
using Base.Time;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SpawnableTile
{
    public TileBase tile;
    public Vector3Int prefabsTilemapPosition;
}

public class LandscapeController : MonoBehaviour
{
    [SerializeField] private Collider2D safeFromObjectSpawningArea;
    [SerializeField] public Tilemap prefabsTilemap;
    [SerializeField] private SpawnableTile[] availableToSpawnTiles;

    public static LandscapeController Instance;

    public readonly Dictionary<TileBase, List<Vector3Int>> SpawnableTilesDictionary = new();

    private Vector3Int _minPosition;
    private Vector3Int _maxPosition;
    private Unity.Mathematics.Random _random;
    private TileMapReadManager _tileMapReadManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
        _tileMapReadManager = TileMapReadManager.Instance;

        var cellBounds = _tileMapReadManager.backgroundTilemap.cellBounds;

        _minPosition = cellBounds.min;
        _maxPosition = cellBounds.max;

        Debug.Log(_minPosition);
        Debug.Log(_maxPosition);

        foreach (var tile in availableToSpawnTiles)
            WorldTimer.AddOnDayChangedHandler(() => SpawnTile(tile));
    }

    private void SpawnTile(SpawnableTile tile)
    {
        var existingTile = tile.tile;
        Vector3Int randomPosition = default;

        var triesAvailable = 100;

        while (existingTile != null)
        {
            if (triesAvailable-- == 0)
                return;

            randomPosition = new Vector3Int(
                _random.NextInt(_minPosition.x, _maxPosition.x),
                _random.NextInt(_minPosition.y, _maxPosition.y));

            var spawnWorldPosition = TileMapReadManager.GetCellCenterWorldPosition(
                _tileMapReadManager.backgroundTilemap,
                randomPosition);

            if (safeFromObjectSpawningArea.bounds.Contains(spawnWorldPosition))
                continue;

            existingTile = _tileMapReadManager.landscapeTilemap.GetTile(randomPosition);
        }

        _tileMapReadManager.landscapeTilemap.SetTile(
            new TileChangeData(
                randomPosition,
                tile.tile,
                Color.white,
                prefabsTilemap.GetTransformMatrix(tile.prefabsTilemapPosition)),
            false);

        if (!SpawnableTilesDictionary.ContainsKey(tile.tile))
            SpawnableTilesDictionary[tile.tile] = new List<Vector3Int>();

        SpawnableTilesDictionary[tile.tile].Add(randomPosition);

        Debug.Log($"Spawned [{tile.tile.name}] on coordinates {randomPosition}.");
    }
}
