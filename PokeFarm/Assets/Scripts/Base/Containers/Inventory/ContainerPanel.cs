using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerPanel : MonoBehaviour
{
    [SerializeField] protected ItemContainer itemContainer;
    [SerializeField] protected List<ContainerButton> inventoryButtons;
    [SerializeField] private ContainerButton containerButtonPrefab;

    public int ButtonsCount => inventoryButtons.Count;

    private bool _isInitialized;

    public void Init(ItemContainer container)
    {
        itemContainer = container;

        foreach (var _ in container.Slots)
        {
            var inventoryButton = Instantiate(containerButtonPrefab, transform, false);
            inventoryButtons.Add(inventoryButton);
        }

        _isInitialized = true;
    }

    public void Refresh()
    {
        if (!_isInitialized)
        {
            Close();
            return;
        }

        for (var i = 0; i < itemContainer.slotsCount && i < inventoryButtons.Count; i++)
        {
            var currentSlot = itemContainer.Slots[i];

            if (currentSlot.item == null)
            {
                inventoryButtons[i].Clean();
                continue;
            }

            inventoryButtons[i].Set(currentSlot);
        }
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
        for (var i = 0; i < itemContainer.Slots.Count && i < inventoryButtons.Count; i++)
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

    public virtual void OnClick(int id, PointerEventData.InputButton inputButton) { }
}