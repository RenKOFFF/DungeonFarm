using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    public Item _item;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if(_item)
            _sprite.sprite = _item.icon;
    }

    public void RefreshItem(Item item)
    {
        if (!item) return;
        
        _item = item;
        _sprite.sprite = _item.icon;
    }
}
