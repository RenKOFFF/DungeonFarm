using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase markerTile;

    public static MarkerManager Instance { get; private set; }

    public Vector3Int markedCellPosition;
    private Vector3Int oldCellPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        markedCellPosition = TileMapReadManager.Instance.GetCurrentBackgroundGridPositionByMousePosition();

        targetTilemap.SetTile(oldCellPosition, null);
        targetTilemap.SetTile(markedCellPosition, markerTile);
        oldCellPosition = markedCellPosition;
    }
}
