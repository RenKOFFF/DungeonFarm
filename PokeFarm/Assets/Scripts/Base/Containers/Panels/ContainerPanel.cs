using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerPanel : MonoBehaviour
{
    [SerializeField] protected ContainerController containerController;
    [SerializeField] protected List<ContainerButton> inventoryButtons;

    public int ButtonsCount => inventoryButtons.Count;

    public virtual void Refresh()
    {
        if (containerController.Slots == null)
            return;

        for (var i = 0; i < containerController.slotsCount && i < inventoryButtons.Count; i++)
        {
            var currentSlot = containerController.Slots[i];

            if (currentSlot.item == null)
            {
                inventoryButtons[i].Clean();
                continue;
            }

            inventoryButtons[i].Set(currentSlot);
        }

        containerController.Save();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void SetButtonIndexes()
    {
        for (var i = 0; i < containerController.Slots.Count && i < inventoryButtons.Count; i++)
            inventoryButtons[i].SetIndex(i);
    }

    protected void Start()
    {
        SetButtonIndexes();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public virtual void OnClick(int id, PointerEventData.InputButton inputButton)
    {
        GameManager.Instance.dragAndDropController.OnClick(containerController.Slots[id], inputButton);
        Refresh();
    }
}
