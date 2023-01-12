using Base.GameData;
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

        var worldData = GameDataController.LoadWithInitializationIfEmpty<WorldData>(
            DataCategory.WorldData,
            WorldData.SaveName);

        if (worldData.IsInitialized)
            return;

        worldData.IsInitialized = true;
        GameDataController.Save(worldData, DataCategory.WorldData, WorldData.SaveName);

        // TODO заменить на выдачу определённых вещей, а не всех
        foreach (var (_, item) in GameDataController.AllItems)
            GameManager.Instance.inventory.Add(item, 20);
    }

    private void Update()
    {
        if (GameManager.Instance.HasDifferentActiveUIPanel(inventoryPanel.gameObject))
            return;

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
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
