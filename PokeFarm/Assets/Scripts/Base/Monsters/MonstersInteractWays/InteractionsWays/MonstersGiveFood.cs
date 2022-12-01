using UnityEngine;

public class MonstersGiveFood : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    public override void Execute()
    {
        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        Item itemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;
        {
            _monsterBehaviour.Monster.Inventory.AddItem(itemOnTheHand);
            GameManager.Instance.inventory.Remove(itemOnTheHand);
        }

        Debug.Log($"{_monsterBehaviour.Monster.Inventory.GetItem()?.Name}");

        gameObject.SetActive(false);
    }

    public override bool GetDisplayCondition()
    {
        Item itemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;
        return (itemOnTheHand != null && itemOnTheHand.type is ItemType.Food);
    }

    private void Awake()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        gameObject.SetActive(false);
    }
}
