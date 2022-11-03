using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public GameObject settingsPanel;
    public GameObject activeUIPanel;

    public bool HasActiveUIPanel => activeUIPanel != null && activeUIPanel.activeInHierarchy;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Instance.HasActiveUIPanel && Instance.activeUIPanel != settingsPanel)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingsPanel.SetActive(!settingsPanel.activeInHierarchy);

            if (settingsPanel.activeInHierarchy)
                activeUIPanel = settingsPanel;
        }
    }
}
