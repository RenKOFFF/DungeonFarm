using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] protected ItemContainer inventory;
    [SerializeField] protected List<InventoryButton> buttons;

    public void Refresh()
    {
        for (var i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
        {
            var currentSlot = inventory.slots[i];

            if (currentSlot.item == null)
            {
                buttons[i].Clean();
                continue;
            }

            buttons[i].Set(currentSlot);
        }
    }

    private void SetButtonIndexes()
    {
        for (var i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
            buttons[i].SetIndex(i);
    }

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        SetButtonIndexes();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public virtual void OnClick(int id) { }
}
