using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersGiveFood : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    public override void Execute()
    {
        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        if (ToolbarManager.Instance.ItemOnTheHand.isFood)
        {
            _monsterBehaviour.Monster.Inventory.AddItem(ToolbarManager.Instance.ItemOnTheHand);
            //TODO delete this item from player inventory
        }

        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        gameObject.SetActive(false);
    }

    public override bool GetDisplayCondition()
    {
        return true;
    }

    private void Awake()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        gameObject.SetActive(false);
    }
}
