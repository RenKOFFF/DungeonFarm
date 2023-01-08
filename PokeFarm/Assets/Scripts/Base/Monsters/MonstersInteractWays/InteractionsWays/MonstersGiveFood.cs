using UnityEngine;

public class MonstersGiveFood : MonstersInteractionWay
{
    private MonsterBehaviour _monsterBehaviour;

    public override void Execute()
    {
        Item itemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;
        {
            _monsterBehaviour.Monster.Inventory.AddItem(itemOnTheHand);
            GameManager.Instance.inventory.Remove(itemOnTheHand);
            _monsterBehaviour.Monster.Hunger.FeelFull();
            
            Debug.Log(_monsterBehaviour.Monster.Hunger.SatietyLevel);
        }

        gameObject.SetActive(false);
    }

    public override bool GetDisplayCondition()
    {
        Item itemOnTheHand = ToolbarManager.Instance.ItemOnTheHand;
        return itemOnTheHand != null && itemOnTheHand.type is ItemType.Food && !_monsterBehaviour.Monster.Hunger.IsFedToday;
    }

    private void Awake()
    {
        _monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
        gameObject.SetActive(false);
    }
}
