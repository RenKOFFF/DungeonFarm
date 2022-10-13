using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 12;
    private int selectedToolbarItemSlotIndex;

    public UnityEvent<int> OnSelectChangeEvent = new UnityEvent<int>();

    public void Set(int id)
    {
        selectedToolbarItemSlotIndex = id;
        //OnSelectChangeEvent.Invoke(selectedToolbarItemSlotIndex);
    }

    void Update()
    {
        float mouseScrollDelta = Input.mouseScrollDelta.y;
        if (mouseScrollDelta != 0)
        {
            if (mouseScrollDelta < 0)
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
}
