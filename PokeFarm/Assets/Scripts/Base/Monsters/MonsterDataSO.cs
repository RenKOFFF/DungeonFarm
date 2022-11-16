using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monsters Data/Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    public string MonsterName { get => _monsterName; }
    public Sprite Icon { get => _icon; }
    public MonstersInteractionWayDataSO[] InteractionWay { get => _interactionWay; }

    [SerializeField] private string _monsterName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private MonstersInteractionWayDataSO[] _interactionWay;
}