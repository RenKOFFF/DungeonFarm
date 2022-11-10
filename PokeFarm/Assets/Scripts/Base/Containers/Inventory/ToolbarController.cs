using UnityEngine;
using UnityEngine.Events;

public class ToolbarController : MonoBehaviour
{
    public static ToolbarController Instance { get; private set; }

    [SerializeField] private ToolbarPanel currentToolbar;

    public Item ItemOnTheHand { get; private set; }

    public UnityEvent<int> OnSelectedSlotIndexChanged { get; } = new();
    public UnityEvent OnItemOnTheHandChanged { get; } = new();

    private int _toolbarSize;
    private int _selectedToolbarSlotIndex;

    public void SetSlotIndex(int id)
    {
        _selectedToolbarSlotIndex = id;
        ChangeItemOnHand();
    }

    private void ChangeItemOnHand()
    {
        OnSelectedSlotIndexChanged.Invoke(_selectedToolbarSlotIndex);
        ItemOnTheHand = currentToolbar.GetCurrentSelectedItem();
        OnItemOnTheHandChanged.Invoke();
    }

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

    private void Start()
    {
        ItemOnTheHand = currentToolbar.GetCurrentSelectedItem();
        _toolbarSize = currentToolbar.ButtonsCount;
    }

    private void Update()
    {
        var mouseScrollDelta = Input.mouseScrollDelta.y;
        if (mouseScrollDelta == 0) return;

        _selectedToolbarSlotIndex += mouseScrollDelta > 0 ? -1 : 1;
        _selectedToolbarSlotIndex = (_selectedToolbarSlotIndex + _toolbarSize) % _toolbarSize;

        ChangeItemOnHand();
    }
}
