using Base.Monsters;
using UnityEngine;
using UnityEngine.UI;

public class AutobattleMonster : MonoBehaviour
{
    [field: SerializeField] public Slider HealthSlider { get; set; }
    [field: SerializeField] public Slider EnergySlider { get; set; }

    [field: SerializeField] public float TimeToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] public float TimeLeftToMakeMoveInSeconds { get; set; } = 1;

    public bool IsDead => Health == 0;
    public bool IsReadyToAttack => TimeLeftToMakeMoveInSeconds <= 0;

    private MonsterStats Stats { get; set; }
    private float Health { get; set; }

    public void Initialize(MonsterStats stats)
    {
        Stats = stats;
        Health = stats.Health;

        HealthSlider.maxValue = Health;
        UpdateHealthSliderValue();

        EnergySlider.maxValue = TimeToMakeMoveInSeconds;
        UpdateEnergySliderValue();
    }

    public void ReduceTimeLeftToMakeMove(float seconds)
    {
        TimeLeftToMakeMoveInSeconds -= seconds;
        UpdateEnergySliderValue();
    }

    public void Attack(AutobattleMonster enemy)
    {
        enemy.GetDamage(Stats.Strength);
        TimeLeftToMakeMoveInSeconds = TimeToMakeMoveInSeconds;
    }

    private void GetDamage(float enemyStrength)
    {
        Health -= enemyStrength;

        if (Health < 0)
            Health = 0;

        UpdateHealthSliderValue();
    }

    private void UpdateHealthSliderValue()
    {
        HealthSlider.value = Health;
    }

    private void UpdateEnergySliderValue()
    {
        EnergySlider.value = TimeToMakeMoveInSeconds - TimeLeftToMakeMoveInSeconds;
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
