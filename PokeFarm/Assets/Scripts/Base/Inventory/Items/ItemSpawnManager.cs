using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject pickUpItemPrefab;

    public void SpawnItem(Vector2 position, Item item, int count)
    {
        var obj = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        obj.GetComponent<PickUpItem>().Set(item, count);
    }
}
