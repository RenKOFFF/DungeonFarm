using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickUpItemPrefab;

    public static SpawnManager Instance;

    public void SpawnItem(Vector2 position, Item item, int amount)
    {
        var obj = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        obj.GetComponent<PickUpItem>().Set(item, amount);
    }

    public void SpawnLandscapeObject(Tilemap targetTilemap, Vector3Int position, TileBase tileBase)
    {
        targetTilemap.SetTile(position, tileBase);
    }

    private void Awake()
    {
        Instance = this;
    }
}
