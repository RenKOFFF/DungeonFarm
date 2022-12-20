using Base.Monsters;
using UnityEngine;

public class AutobattleMonster : MonoBehaviour
{
    [field: SerializeField] public float TimeToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] public float TimeLeftToMakeMoveInSeconds { get; set; } = 1;

    public bool IsDead => Health == 0;

    private MonsterStats Stats { get; set; }
    private float Health { get; set; }

    public void Initialize(MonsterStats stats)
    {
        Stats = stats;
        Health = stats.Health;
    }

    public void Attack(AutobattleMonster enemy)
    {
        enemy.GetDamage(Stats.Strength);
    }

    private void GetDamage(float enemyStrength)
    {
        Health -= enemyStrength;

        if (Health < 0)
            Health = 0;

        Debug.Log($"[-{enemyStrength} hp] Ouch!. {Health} hp left.");
    }

    public void Die()
    {
        Debug.Log("I'm dead!!");
    }
}
