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