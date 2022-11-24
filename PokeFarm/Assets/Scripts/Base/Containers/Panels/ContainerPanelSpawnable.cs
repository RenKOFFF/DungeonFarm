using UnityEngine;

public class ContainerPanelSpawnable : ContainerPanel
{
    [SerializeField] private ContainerButton containerButtonPrefab;

    private bool _isInitialized;

    public void Init(ContainerController containerController)
    {
        this.containerController = containerController;

        foreach (var _ in containerController.Slots)
        {
            var inventoryButton = Instantiate(containerButtonPrefab, transform, false);
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
