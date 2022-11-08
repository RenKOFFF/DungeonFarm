using UnityEngine;

public class ToolsController : MonoBehaviour
{
    private CharacterController2D _characterController2D;

    [SerializeField] private float offsetDistance = 1f;
    [SerializeField] private float interactableAreaSize = 1.2f;

    private void Awake()
    {
        _characterController2D = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var currentItemOnTheHand = HandBank.instance.itemOnTheHand;

        if (currentItemOnTheHand == null || currentItemOnTheHand.type != ItemType.Tool)
            return;

        var landscapeTilemap = TileMapReadManager.Instance.landscapeTilemap;
        var currentGridPosition = TileMapReadManager.GetGridPosition(landscapeTilemap, Input.mousePosition, true);
        var tileData = TileMapReadManager.Instance.GetLandscapeTileDataByMousePosition();

        if (!tileData.IsBreakable)
            return;

        var breakableTile = tileData.BreakableTile;

        if (currentItemOnTheHand != breakableTile.breaksByTool)
            return;

        landscapeTilemap.SetTile(
            currentGridPosition,
            null);
    }
}
