using Base.Monsters;
using UnityEngine;
using UnityEngine.UI;

public class AutobattleMonster : MonoBehaviour
{
    [field: SerializeField] public Slider HealthSlider { get; set; }
    [field: SerializeField] public Slider EnergySlider { get; set; }

    [field: SerializeField] public float TimeToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] public float TimeLeftToMakeMoveInSeconds { get; set; } = 1;
    [field: SerializeField] public float AttackTimeInSeconds { get; set; } = 0.3f;

    public bool IsDead => Health == 0;
    public bool IsReadyToAttack => TimeLeftToMakeMoveInSeconds <= 0;

    private bool NeedPlayAttackAnimation { get; set; }
    private bool NeedPlayReturnFromAttackAnimation { get; set; }

    private float MinimalDistanceToEndAnimation { get; set; } = 0.01f;
    private Vector3 StartAnimationPosition { get; set; }
    private AutobattleMonster EnemyMonster { get; set; }
    private float AnimationSpeed { get; set; }

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
        PlayAttackAnimation(enemy);

        enemy.GetDamage(Stats.Strength);
        TimeLeftToMakeMoveInSeconds = TimeToMakeMoveInSeconds;
    }

    private void PlayAttackAnimation(AutobattleMonster autobattleMonster)
    {
        var endAnimationPosition = autobattleMonster.transform.position;

        EnemyMonster = autobattleMonster;
        StartAnimationPosition = transform.position;
        AnimationSpeed = Vector3.Distance(StartAnimationPosition, endAnimationPosition) / AttackTimeInSeconds * 2;

        NeedPlayAttackAnimation = true;
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

    private void Update()
    {
        if (NeedPlayAttackAnimation)
        {
            var endAnimationPosition = EnemyMonster.transform.position;

            transform.position = Vector3.MoveTowards(
                transform.position,
                endAnimationPosition,
                AnimationSpeed * Time.deltaTime);

            var distanceToEndAnimation = Vector3.Distance(transform.position, endAnimationPosition);

            if (distanceToEndAnimation < MinimalDistanceToEndAnimation)
            {
                NeedPlayAttackAnimation = false;
                NeedPlayReturnFromAttackAnimation = true;
            }

            return;
        }

        if (NeedPlayReturnFromAttackAnimation)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                StartAnimationPosition,
                AnimationSpeed * Time.deltaTime);

            var distanceToEndAnimation = Vector3.Distance(transform.position, StartAnimationPosition);

            if (distanceToEndAnimation < MinimalDistanceToEndAnimation)
                NeedPlayReturnFromAttackAnimation = false;
        }
    }
}
