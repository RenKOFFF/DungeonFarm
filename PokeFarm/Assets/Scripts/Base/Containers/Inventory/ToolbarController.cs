using UnityEngine;
using UnityEngine.Events;

public class ToolbarController : MonoBehaviour
{
    public static ToolbarController instanse;

    private int toolbarSize = 12;
    private int selectedToolbarItemSlotIndex;

    [HideInInspector] public UnityEvent<int> OnSelectChangeEvent = new UnityEvent<int>();

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = this;
            DontDestroyOnLoad(instanse);
            return;
        }

        Destroy(gameObject);
    }
    void Update()
    {
        float mouseScrollDelta = Input.mouseScrollDelta.y;
        if (mouseScrollDelta != 0)
        {
            if (mouseScrollDelta > 0)
            {
                selectedToolbarItemSlotIndex = selectedToolbarItemSlotIndex - 1 < 0 ? toolbarSize - 1 : --selectedToolbarItemSlotIndex;
            }
            else
            {
                selectedToolbarItemSlotIndex = selectedToolbarItemSlotIndex + 1 > toolbarSize - 1 ? 0 : ++selectedToolbarItemSlotIndex;
            }
            OnSelectChangeEvent.Invoke(selectedToolbarItemSlotIndex);
        }
    }

    public void Set(int id)
    {
        selectedToolbarItemSlotIndex = id;
        OnSelectChangeEvent.Invoke(selectedToolbarItemSlotIndex);
    }
}
