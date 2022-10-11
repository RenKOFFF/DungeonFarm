using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Item currentItem;
    private int currentCount = 1;

    public void Set(Item item, int count)
    {
        currentItem = item;
        currentCount = count;

        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    // TODO
}
