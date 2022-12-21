namespace Base.Monsters
{
    public struct MonsterStats
    {
        public float Strength, Speed, Health, Luck, Defense, Dexterity;
        public int InventorySize; 

        public MonsterStats(float strength, float luck, float defense, float health, float dexterity, float speed, int inventorySize)
        {
            Strength = strength;
            Luck = luck;
            Defense = defense;
            Health = health;
            Dexterity = dexterity;
            
            Speed = speed;
            InventorySize = inventorySize;
        }
    }
}