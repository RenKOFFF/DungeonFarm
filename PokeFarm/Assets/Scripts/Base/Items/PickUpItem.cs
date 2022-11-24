using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float pickUpDistance = 0.8f;

    private Transform _player;
    private Item _currentItem;
    private int _currentAmount = 1;

    public void Set(Item item, int amount)
    {
        _currentItem = item;
        _currentAmount = amount;

        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    private void Awake()
    {
        _player = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, _player.position);

        if (distance > pickUpDistance)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);

        if (distance < 0.1)
            AddItemToInventory();
    }

    private void AddItemToInventory()
    {
        GameManager.Instance.inventory.Add(_currentItem, _currentAmount);
        InventoryManager.Instance.Refresh();
        Destroy(gameObject);
    }
}
