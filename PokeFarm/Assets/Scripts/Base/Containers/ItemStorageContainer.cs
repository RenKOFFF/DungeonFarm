using UnityEngine;
using UnityEngine.Events;

public class ItemStorageContainer
{
    public Item[] Container { get; private set; }
    public UnityEvent OnContainerChangedContentEvent = new UnityEvent();

    public ItemStorageContainer(int containerSize)
    {
        Container = new Item[containerSize];
    }
//TODO fix this
    public void AddItem(Item item)
    {
        Container[0] = item;
        OnContainerChangedContentEvent.Invoke();
    }

    public void MoveItemToInventory(int index = 0)
    {
        var item = Container[index];
        if (item)
        {
            GameManager.Instance.inventory.Add(item);
            Container[index] = null;
            OnContainerChangedContentEvent.Invoke();
        }
        else Debug.Log("Item is null");
    }

    public Item GetItem()
    {
        return Container[0];
    }
}
