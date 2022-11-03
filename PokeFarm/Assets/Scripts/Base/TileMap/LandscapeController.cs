using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SpawnableTile
{
    public TileBase tile;
    public float spawnIntervalInSeconds = 10;
    public Vector3Int prefabTilemapPosition;

    [NonSerialized] public float TimeLeftToSpawnRockInSeconds;
}

public class LandscapeController : MonoBehaviour
{
    [SerializeField] private SpawnableTile[] availableToSpawnTiles;
    [SerializeField] private Collider2D safeFromObjectSpawningArea;
    [SerializeField] public Tilemap prefabTilemap;

    private Vector3Int _minPosition;
    private Vector3Int _maxPosition;
    private Unity.Mathematics.Random _random;
    private TileMapReadManager _tileMapReadManager;

    private void Start()
    {
        foreach (var tile in availableToSpawnTiles)
            tile.TimeLeftToSpawnRockInSeconds = tile.spawnIntervalInSeconds;

        _random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
        _tileMapReadManager = TileMapReadManager.Instance;

        var cellBounds = _tileMapReadManager.backgroundTilemap.cellBounds;

        _minPosition = cellBounds.min;
        _maxPosition = cellBounds.max;
    }

    private void Update()
    {
        foreach (var tile in availableToSpawnTiles)
            TrySpawnTile(tile);
    }

    private void TrySpawnTile(SpawnableTile tile)
    {
        tile.TimeLeftToSpawnRockInSeconds -= Time.deltaTime;

        if (tile.TimeLeftToSpawnRockInSeconds < 0)
        {
            SpawnTile(tile);
            tile.TimeLeftToSpawnRockInSeconds = tile.spawnIntervalInSeconds;
        }
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

            var spawnWorldPosition = _tileMapReadManager.backgroundTilemap.CellToWorld(randomPosition) +
                                     _tileMapReadManager.backgroundTilemap.layoutGrid.cellSize / 2;

            if (safeFromObjectSpawningArea.bounds.Contains(spawnWorldPosition))
                continue;

            existingTile = _tileMapReadManager.landscapeTilemap.GetTile(randomPosition);
        }

        _tileMapReadManager.landscapeTilemap.SetTile(
            new TileChangeData(
                randomPosition,
                tile.tile,
                Color.white,
                prefabTilemap.GetTransformMatrix(tile.prefabTilemapPosition)),
            false);

        Debug.Log($"Spawned [{tile.tile.name}] on coordinates {randomPosition}.");
    }
}
