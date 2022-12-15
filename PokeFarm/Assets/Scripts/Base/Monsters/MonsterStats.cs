namespace Base.Monsters
{
    public struct MonsterStats
    {
        public float Strength, Speed, Health, Luck, Protection, Agility;
        public int InventorySize; 

        public MonsterStats(float strength, float luck, float protection, float health, float agility, float speed, int inventorySize)
        {
            Strength = strength;
            Luck = luck;
            Protection = protection;
            Health = health;
            Agility = agility;
            
            Speed = speed;
            InventorySize = inventorySize;
        }
    }
}