using Base.CommandStation.Commands;
using Base.Managers;
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
}
