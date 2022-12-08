namespace Base.Monsters
{
    public struct MonsterStats
    {
        public float Strength, Speed, Health;
        public int InventorySize; 

        public MonsterStats(float strength, float speed, float health, int inventorySize)
        {
            Strength = strength;
            Speed = speed;
            Health = health;
            InventorySize = inventorySize;
        }
    }
}