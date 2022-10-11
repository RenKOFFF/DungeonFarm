using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragAndDropController : MonoBehaviour
{
    private ItemSlot itemSlot;

    private void Start()
    {
        itemSlot = new ItemSlot();
    }
}
