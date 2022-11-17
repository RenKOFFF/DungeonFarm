using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private ToolbarPanel toolbarPanel;

    public static InventoryManager Instance { get; private set; }

    public void Refresh()
    {
        inventoryPanel.Refresh();
        toolbarPanel.Refresh();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inventoryPanel.gameObject.SetActive(false);
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
