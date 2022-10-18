using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private ToolbarPanel toolbarPanel;

    private void Start()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeInHierarchy);
            toolbarPanel.gameObject.SetActive(!toolbarPanel.gameObject.activeInHierarchy);
        }
    }
}
