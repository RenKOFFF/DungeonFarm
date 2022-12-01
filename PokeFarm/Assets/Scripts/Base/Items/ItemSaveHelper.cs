﻿using System.Collections.Generic;
using System.Linq;

namespace Base.Items
{
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
            => slotSaveItem.ItemName == null
                ? new ItemSlot()
                : new ItemSlot(GameDataController.AllItems[slotSaveItem.ItemName], slotSaveItem.Amount);
    }
}
