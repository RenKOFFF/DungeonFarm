using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase markerTile;

    public Vector3Int markedCellPosition;
    private Vector3Int oldCellPosition;

    private void Update()
    {
        markedCellPosition = GameManager.Instance.tileMapReader.GetCurrentGridPositionByMousePosition();

        targetTilemap.SetTile(oldCellPosition, null);
        targetTilemap.SetTile(markedCellPosition, markerTile);
        oldCellPosition = markedCellPosition;
    }
}
