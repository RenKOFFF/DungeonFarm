using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject pickUpItemPrefab;

    public static SpawnManager Instance { get; private set; }

    private Unity.Mathematics.Random random;

    public void SpawnPickUpItem(Vector2 position, Item item, int amount)
    {
        var obj = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        obj.GetComponent<PickUpItem>().Set(item, amount);
    }

    public void SpawnPickUpItemsInArea(Vector2 centerPosition, Item item, int amount, float spread = 0.5f)
    {
        var maxPickUpAmount = (int) Math.Floor(Math.Sqrt(amount));

        while (amount > 0)
        {
            var spawnAmount = Math.Min(amount, maxPickUpAmount);
            var spawnPosition = centerPosition;

            spawnPosition.x += spread * random.NextFloat(-1, 1);
            spawnPosition.y += spread * random.NextFloat(-1, 1);

            SpawnPickUpItem(spawnPosition, item, spawnAmount);

            amount -= spawnAmount;
        }
    }

    public void SpawnObject(Vector2 position, GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, position, Quaternion.identity);
    }

    private void Awake()
    {
        Instance = this;
        random = new Unity.Mathematics.Random((uint) DateTime.Now.Millisecond);
    }
}
