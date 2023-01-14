using UnityEngine;

public class ContainerPanelSpawnable : ContainerPanel
{
    [SerializeField] private ContainerButton containerButtonPrefab;
    [SerializeField] private ContainerButton toolbarButtonPrefab;
    [SerializeField] private Transform _parentContainerButtons;
    [SerializeField] private Transform _parentToolbarContainerButtons;

    private bool _isInitialized;

    public void Init(ContainerController containerController)
    {
        this.containerController = containerController;

        var ToolbarButtonsCount = ToolbarManager.Instance.ToolbarSize;

        for (int i = 0; i < containerController.Slots.Count; i++)
        {
            ContainerButton inventoryButton;
            if (i < ToolbarButtonsCount)
            {
                inventoryButton = Instantiate(toolbarButtonPrefab, _parentToolbarContainerButtons, false);
            }
            else
            {
                inventoryButton = Instantiate(containerButtonPrefab, _parentContainerButtons, false);
            }
            inventoryButtons.Add(inventoryButton);
        }

        _isInitialized = true;
    }

    public override void Refresh()
    {
        if (!_isInitialized)
        {
            Close();
            return;
        }

        base.Refresh();
    }
}
