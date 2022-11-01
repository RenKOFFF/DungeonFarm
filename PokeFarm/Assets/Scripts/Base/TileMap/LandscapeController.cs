using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandscapeController : MonoBehaviour
{
    [SerializeField] private float rocksSpawnIntervalInSeconds = 10;
    [SerializeField] private TileBase rockTile;
    [SerializeField] private GameObject[] availableToSpawnObjects;
    [SerializeField] private Collider2D safeFromObjectSpawningArea;

    private Vector3Int _minPosition;
    private Vector3Int _maxPosition;
    private float _timeLeftToSpawnRockInSeconds;
    private Unity.Mathematics.Random _random;
    private TileMapReadManager _tileMapReadManager;

    private readonly List<Vector3Int> _notAvailablePositions = new();

    private void Start()
    {
        _timeLeftToSpawnRockInSeconds = rocksSpawnIntervalInSeconds;
        _random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
        _tileMapReadManager = TileMapReadManager.Instance;

        var cellBounds = _tileMapReadManager.backgroundTilemap.cellBounds;

        _minPosition = cellBounds.min;
        _maxPosition = cellBounds.max;
    }

    private void Update()
    {
        TrySpawnRock();
    }

    private void TrySpawnRock()
    {
        _timeLeftToSpawnRockInSeconds -= Time.deltaTime;

        if (_timeLeftToSpawnRockInSeconds < 0)
        {
            SpawnObjectAsTile(rockTile);
            // SpawnObjectAsInstance();
            _timeLeftToSpawnRockInSeconds = rocksSpawnIntervalInSeconds;
        }
    }

    private void SpawnObjectAsTile(TileBase tileBase)
    {
        var existingTile = tileBase;
        Vector3Int randomPosition = default;

        var triesAvailable = 100;

        while (existingTile != null)
        {
            if (triesAvailable-- == 0)
                return;

            randomPosition = new Vector3Int(
                _random.NextInt(_minPosition.x, _maxPosition.x),
                _random.NextInt(_minPosition.y, _maxPosition.y));

            existingTile = _tileMapReadManager.landscapeTilemap.GetTile(randomPosition);
        }

        SpawnManager.Instance.SpawnLandscapeObject(_tileMapReadManager.landscapeTilemap, randomPosition, tileBase);
        Debug.Log($"Spawned [{tileBase}] in [{randomPosition}].");
    }

    private void SpawnObjectAsInstance()
    {
        Vector3Int randomPosition;
        var triesAvailable = 100;
        Vector3 spawnWorldPosition;

        while (true)
        {
            if (triesAvailable-- == 0)
                return;

            randomPosition = new Vector3Int(
                _random.NextInt(_minPosition.x, _maxPosition.x),
                _random.NextInt(_minPosition.y, _maxPosition.y));

            spawnWorldPosition = _tileMapReadManager.backgroundTilemap.CellToWorld(randomPosition) +
                                 _tileMapReadManager.backgroundTilemap.layoutGrid.cellSize / 2;

            if (safeFromObjectSpawningArea.bounds.Contains(spawnWorldPosition))
                continue;

            if (!_notAvailablePositions.Contains(randomPosition))
                break;
        }

        var randomObject = availableToSpawnObjects[_random.NextInt(availableToSpawnObjects.Length)];

        SpawnManager.Instance.SpawnObject(spawnWorldPosition, randomObject);
        _notAvailablePositions.Add(randomPosition);
        Debug.Log($"Spawned [{randomObject}] in [{randomPosition}].");
    }
}
