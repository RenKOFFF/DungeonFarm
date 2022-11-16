using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersGetItem : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    public override void Execute()
    {
        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        _monsterBehaviour.Monster.Inventory.MoveItemToInventory();

        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _monsterBehaviour.Monster.Inventory.OnContainerChangedContentEvent.AddListener(CheckDisplayCondition);
    }

    private void OnDisable()
    {
        _monsterBehaviour.Monster.Inventory.OnContainerChangedContentEvent.RemoveListener(CheckDisplayCondition);
    }

    private void Awake()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        CheckDisplayCondition();

        gameObject.SetActive(false);
    }

    private void CheckDisplayCondition()
    {
        _displayCondition = GetDisplayCondition();
    }

    public override bool GetDisplayCondition()
    {
        return _monsterBehaviour.Monster.Inventory.Container.Length > 0 && _monsterBehaviour.Monster.Inventory.Container[0] != null;
    }
}
