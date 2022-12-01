namespace Base.Items
{
    public class SlotSaveItem
    {
        public string ItemName { get; }
        public int Amount { get; }

        public SlotSaveItem(string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
        }
    }
}