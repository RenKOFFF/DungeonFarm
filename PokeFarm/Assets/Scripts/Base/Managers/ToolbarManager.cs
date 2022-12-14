using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class ToolbarManager : MonoBehaviour
{
    public static ToolbarManager Instance { get; private set; }

    [SerializeField] private ToolbarPanel currentToolbar;

    [CanBeNull] public Item ItemOnTheHand { get; private set; }

    public UnityEvent<int> OnSelectedSlotIndexChanged { get; } = new();
    public UnityEvent OnItemOnTheHandChanged { get; } = new();

    private const string NumericKeyboardButtonsKeyCodeName = "Alpha";

    private int _toolbarSize;
    private int _selectedToolbarSlotIndex;

    public void SetSlotIndex(int index)
    {
        _selectedToolbarSlotIndex = (index + _toolbarSize) % _toolbarSize;
        ChangeItemOnHand();
    }

    private void ChangeItemOnHand()
    {
        OnSelectedSlotIndexChanged.Invoke(_selectedToolbarSlotIndex);
        ItemOnTheHand = currentToolbar.GetCurrentSelectedItem();
        OnItemOnTheHandChanged.Invoke();
    }

    public void RefreshItemOnTheHand()
    {
        ChangeItemOnHand();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ItemOnTheHand = currentToolbar.GetCurrentSelectedItem();
        _toolbarSize = currentToolbar.ButtonsCount;
    }

    private void Update()
    {
        var mouseScrollDelta = Input.mouseScrollDelta.y;

        if (mouseScrollDelta == 0)
            return;

        var newIndex = _selectedToolbarSlotIndex + (mouseScrollDelta > 0 ? -1 : 1);

        SetSlotIndex(newIndex);
        ChangeItemOnHand();
    }

    private void OnGUI()
    {
        var currentEvent = Event.current;

        if (currentEvent.type != EventType.KeyDown)
            return;

        var keyCodeString = currentEvent.keyCode.ToString();

        if (!keyCodeString.Contains(NumericKeyboardButtonsKeyCodeName))
            return;

        var indexString = keyCodeString.Replace(NumericKeyboardButtonsKeyCodeName, "");

        if (!int.TryParse(indexString, out var index))
            return;

        SetSlotIndex(--index);
    }
}
