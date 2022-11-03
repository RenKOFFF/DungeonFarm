using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickUpItemPrefab;

    public static SpawnManager Instance { get; private set; }

    public void SpawnPickUpItem(Vector2 position, Item item, int amount)
    {
        var obj = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        obj.GetComponent<PickUpItem>().Set(item, amount);
    }

    public void SpawnObject(Vector2 position, GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, position, Quaternion.identity);
    }

    private void Awake()
    {
        Instance = this;
    }
}
