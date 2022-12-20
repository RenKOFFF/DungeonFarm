using Base.Monsters;
using UnityEngine;
using UnityEngine.UI;

public class AutobattleMonster : MonoBehaviour
{
    [field: SerializeField] public Slider Slider { get; set; }
    [field: SerializeField] public float TimeToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] public float TimeLeftToMakeMoveInSeconds { get; set; } = 1;

    public bool IsDead => Health == 0;

    private MonsterStats Stats { get; set; }
    private float Health { get; set; }

    public void Initialize(MonsterStats stats)
    {
        Stats = stats;
        Health = stats.Health;

        Slider.maxValue = Health;
        Slider.value = Health;
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

        Slider.value = Health;
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
