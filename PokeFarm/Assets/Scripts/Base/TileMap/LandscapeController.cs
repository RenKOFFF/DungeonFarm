using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandscapeController : MonoBehaviour
{
    [SerializeField] private int rocksSpawnIntervalInSeconds = 10;
    [SerializeField] private TileBase rockTile;

    private Vector3Int _minPosition;
    private Vector3Int _maxPosition;
    private float _timeLeftToSpawnRockInSeconds;
    private Unity.Mathematics.Random _random;
    private TileMapReadManager _tileMapReadManager;

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
            SpawnObject(rockTile);
            _timeLeftToSpawnRockInSeconds = rocksSpawnIntervalInSeconds;
        }
    }

    private void SpawnObject(TileBase tileBase)
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
}
