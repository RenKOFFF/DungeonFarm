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

        public override string ToString()
        {
            return $" Сила: {Strength}\n, Скорость: {Speed}\n, ХР: {Health}\n, Удача: {Luck}\n" +
                   $", Броня? {Defense}\n, хз: {Dexterity}\n, Инвентарь: {InventorySize}";
        }
    }
}