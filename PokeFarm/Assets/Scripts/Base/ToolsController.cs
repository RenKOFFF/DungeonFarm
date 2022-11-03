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

        if (HandBank.instance.itemOnTheHand?.type != ItemType.Tool)
            return;

        var tileData = TileMapReadManager.Instance.GetLandscapeTileDataByMousePosition();

        if (tileData.toolInteractable)
        {
            var landscapeTilemap = TileMapReadManager.Instance.landscapeTilemap;

            landscapeTilemap.SetTile(
                TileMapReadManager.GetGridPosition(landscapeTilemap, Input.mousePosition, true),
                null);
        }
    }
}
