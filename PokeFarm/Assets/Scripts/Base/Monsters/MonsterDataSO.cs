using System;
using Base.CommandStation.Commands;
using Base.Managers;
using Base.Monsters;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monsters Data/Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    public string MonsterName { get => _monsterName; }
    public Sprite Icon { get => _icon; }
    public MonstersInteractionWayDataSO[] InteractionWay => _interactionWay;
    public CommandDataSO[] CommandData => _commandDataSO;

    public Monster Instanse => MonstersManager.Instance.GetMonsterInstance(this);
    public Monster Prefab => _prefab;

    [SerializeField] private string _monsterName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private MonstersInteractionWayDataSO[] _interactionWay;
    [SerializeField] private CommandDataSO[] _commandDataSO;
    [SerializeField] private Monster _prefab;

    [Header("Характеристики бесомонов")] 
    [SerializeField] private MonsterStats _startStats;
    private MonsterStats _currentStats;
    

    public MonsterStats Stats
    {
        get => _currentStats;
        private set => _currentStats = value;
    }

    private void Awake()
    {
        _currentStats = _startStats;
    }

    /*[SerializeField, Min(1)] private float _strength;
    [SerializeField, Min(1)] private int _luck;
    [SerializeField, Min(1)] private int _defense;
    [SerializeField, Min(1)] private float _health;
    [SerializeField, Min(1)] private float _dexterity;
    
    [SerializeField, Min(1)] private float _speed;
    [SerializeField, Min(1)] private int _inventorySize;*/
    

    public MonsterStats GetStats()
    {
        return Stats;  //new MonsterStats(_strength, _luck, _defense, _health, _dexterity, _speed, _inventorySize);
    }

    public void SetStats(MonsterStats newStats)
    {
        Stats = newStats;
    }
}
