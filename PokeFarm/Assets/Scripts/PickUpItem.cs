using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float pickUpDistance = 0.5f;

    private Item currentItem;
    private int currentCount = 1;

    public void Set(Item item, int count)
    {
        currentItem = item;
        currentCount = count;

        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    private void Awake()
    {
        player = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, player.position);

        if (distance > pickUpDistance)
            return;

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (distance < 0.1)
        {
            GameManager.Instance.inventoryContainer.Add(currentItem, currentCount);
            Destroy(gameObject);
        }
    }
}
