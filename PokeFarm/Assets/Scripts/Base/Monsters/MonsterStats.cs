using System;
using System.Numerics;

namespace Base.Monsters
{
    [Serializable]
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
        public MonsterStats(MonsterStats stats)
        {
            Strength = stats.Strength;
            Luck = stats.Luck;
            Defense = stats.Defense;
            Health = stats.Health;
            Dexterity = stats.Dexterity;
            
            Speed = stats.Speed;
            InventorySize = stats.InventorySize;
        }
        
        public static MonsterStats operator +(MonsterStats ms1, MonsterStats ms2)
        {
            var newStats = new MonsterStats(
                ms1.Strength + ms2.Strength, 
                ms1.Luck + ms2.Luck,
                ms1.Defense + ms2.Defense,
                ms1.Health + ms2.Health,
                ms1.Dexterity + ms2.Dexterity,
                ms1.Speed + ms2.Speed,
                ms1.InventorySize + ms2.InventorySize);
            return newStats;
        }

        public override string ToString()
        {
            return $" Сила: {Strength},\n Скорость: {Speed},\nХР: {Health}, \nУдача: {Luck},\n" +
                   $"Броня? {Defense},\nхз: {Dexterity},\nИнвентарь: {InventorySize}";
        }
    }
}