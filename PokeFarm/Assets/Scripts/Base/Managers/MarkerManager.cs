using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private TileBase markerTile;
    [SerializeField] private float offsetDistance = 2f;

    public static MarkerManager Instance { get; private set; }

    public Vector3Int markedCellPosition;
    private Vector3Int oldCellPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //TODO temp solve
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainHouseScene")) return;
        
        var playerInstance = GameManager.Instance.player;
        var playerPosition = playerInstance.GetComponent<Rigidbody2D>().position;
        var limitedMouseVector = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerPosition;

        if (limitedMouseVector.magnitude > offsetDistance)
            limitedMouseVector = limitedMouseVector.normalized * offsetDistance;

        var newMarkerPosition = limitedMouseVector + playerPosition;

        markedCellPosition = TileMapReadManager.GetGridPosition(
            TileMapReadManager.Instance.backgroundTilemap,
            newMarkerPosition,
            false);

        targetTilemap.SetTile(oldCellPosition, null);
        targetTilemap.SetTile(markedCellPosition, markerTile);
        oldCellPosition = markedCellPosition;
    }
}
