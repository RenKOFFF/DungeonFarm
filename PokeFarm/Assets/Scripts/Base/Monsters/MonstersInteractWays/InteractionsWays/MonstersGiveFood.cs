using UnityEngine;

public class MonstersGiveFood : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    public override void Execute()
    {
        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        Item itemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;
        if (itemOnTheHand != null && itemOnTheHand.type is ItemType.Food)
        {
            _monsterBehaviour.Monster.Inventory.AddItem(itemOnTheHand);
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
