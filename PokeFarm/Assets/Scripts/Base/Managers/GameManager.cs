using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public GameObject settingsPanel;

    public GameObject ActiveUIPanel { get; set; }
    public bool CanUseEscapeKey { get; set; } = true;

    private bool HasActiveUIPanel => ActiveUIPanel != null && ActiveUIPanel.activeInHierarchy;

    public bool HasDifferentActiveUIPanel(GameObject uiPanelToCompare)
        => HasActiveUIPanel && ActiveUIPanel != uiPanelToCompare;

    private void Awake()
    {
        Instance = this;

        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(Instance);
        //     return;
        // }
        //
        // Destroy(gameObject);
    }

    private void Update()
    {
        if (HasDifferentActiveUIPanel(settingsPanel))
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!CanUseEscapeKey)
            {
                CanUseEscapeKey = true;
                return;
            }

            settingsPanel.SetActive(!settingsPanel.activeInHierarchy);

            if (settingsPanel.activeInHierarchy)
                ActiveUIPanel = settingsPanel;
        }
    }
}
