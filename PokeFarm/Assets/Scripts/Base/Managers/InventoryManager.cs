using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ContainerPanel inventoryPanel;
    [SerializeField] private ToolbarPanel toolbarPanel;
    [SerializeField] private ContainerController containerController;

    public static InventoryManager Instance { get; private set; }

    public void Refresh()
    {
        inventoryPanel?.Refresh();
        toolbarPanel?.Refresh();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inventoryPanel = containerController.SpawnPanel();
        inventoryPanel.Close();
        
        // TODO delete this
        var items = Resources.LoadAll<Item>("Items");

        foreach (var item in items)
        {
            GameManager.Instance.inventory.Add(item, 20);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.HasDifferentActiveUIPanel(inventoryPanel.gameObject))
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchInventoryPanelState();
            return;
        }

        if (inventoryPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.CanUseEscapeKey = false;
            SwitchInventoryPanelState();
        }
    }

    private void SwitchInventoryPanelState()
    {
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeInHierarchy);
        toolbarPanel.gameObject.SetActive(!toolbarPanel.gameObject.activeInHierarchy);

        if (inventoryPanel.gameObject.activeInHierarchy)
            GameManager.Instance.ActiveUIPanel = inventoryPanel.gameObject;
    }
}
