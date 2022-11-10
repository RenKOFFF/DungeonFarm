using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanel : MonoBehaviour
{
    [SerializeField] protected ItemContainer inventory;
    [SerializeField] protected List<InventoryButton> buttons;

    public int ButtonsCount => buttons.Count;

    protected void Refresh()
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

    protected void Init()
    {
        SetButtonIndexes();
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

    private void OnEnable()
    {
        Refresh();
    }

    public virtual void OnClick(int id, PointerEventData.InputButton inputButton) { }
}
