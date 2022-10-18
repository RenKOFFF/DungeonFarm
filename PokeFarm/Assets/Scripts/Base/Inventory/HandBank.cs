using UnityEngine;
using UnityEngine.Events;

public class HandBank : MonoBehaviour
{
    public static HandBank instance;
    [HideInInspector] public Item itemOnTheHand { get; private set; }
    [SerializeField] private ToolbarPanel toolbar;

    [HideInInspector] public UnityEvent OnItemOnTheHandIsSwitchedEvent = new UnityEvent();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        itemOnTheHand = toolbar?.GetCurrentSelectedItem();

        ToolbarController.instanse.OnSelectChangeEvent.AddListener(SwitchItemOnTheHand);
    }

    private void SwitchItemOnTheHand(int index)
    {
        itemOnTheHand = toolbar?.GetCurrentSelectedItem(index);

        OnItemOnTheHandIsSwitchedEvent.Invoke();
    }
}
