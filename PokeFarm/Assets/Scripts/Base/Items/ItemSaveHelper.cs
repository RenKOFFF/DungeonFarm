using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Base.Items
{
    public class SlotSaveItem
    {
        [CanBeNull] public string ItemName { get; }
        public int Amount { get; }

        public SlotSaveItem(string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
        }
    }

    public static class ItemSaveHelper
    {
        public static List<SlotSaveItem> ItemSlotsToSlotSaveItems(IEnumerable<ItemSlot> itemSlots)
            => itemSlots
                .Select(ItemSlotToSlotSaveItem)
                .ToList();

        public static List<ItemSlot> SlotSaveItemsToItemSlots(IEnumerable<SlotSaveItem> itemSlots)
            => itemSlots
                .Select(SlotSaveItemToItemSlot)
                .ToList();

        private static SlotSaveItem ItemSlotToSlotSaveItem(ItemSlot itemSlot)
            => new(itemSlot.item?.Name, itemSlot.amount);

        private static ItemSlot SlotSaveItemToItemSlot(SlotSaveItem slotSaveItem)
        {
            if (slotSaveItem.ItemName == null)
                return new ItemSlot();

            if (!GameDataController.AllItems.ContainsKey(slotSaveItem.ItemName))
            {
                Debug.LogError("Ошибка загрузки предмета." +
                               $" Не удалось найти в игровых файлах предмет с именем [{slotSaveItem.ItemName}].");
                return new ItemSlot();
            }

            return new ItemSlot(GameDataController.AllItems[slotSaveItem.ItemName], slotSaveItem.Amount);
        }
    }
}
