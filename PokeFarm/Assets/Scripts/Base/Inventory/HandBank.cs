using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class HandBank : MonoBehaviour
{
    [HideInInspector] public Item itemOnTheHand { get; private set; }
    [SerializeField] private ToolbarPanel toolbar;
    void Start()
    {
        itemOnTheHand = toolbar?.GetCurrentSelectedItem();

        ToolbarController.instanse.OnSelectChangeEvent.AddListener(SwitchItemOnTheHand);
    }

    private void SwitchItemOnTheHand(int index)
    {
        itemOnTheHand = toolbar?.GetCurrentSelectedItem(index);
    }
}
