using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickUpItemPrefab;
    public static ItemSpawnManager Instance;

    public void SpawnItem(Vector2 position, Item item, int amount)
    {
        var obj = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        obj.GetComponent<PickUpItem>().Set(item, amount);
    }

    private void Awake()
    {
        Instance = this;
    }
}
