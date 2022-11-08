using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MonsterBehaviour))]
public class Monster : MonoBehaviour
{
    [field:SerializeField] public MonsterDataSO MonsterData { get; private set; }
    public float Speed { get => _speed; private set => _speed = value;  }
    [SerializeField] private float _speed = 1.5f;
    public MonsterBehaviour MonsterBehaviour { get => _monsterBehaviour; }
    private MonsterBehaviour _monsterBehaviour;

    private void Awake()
    {
        _monsterBehaviour = GetComponent<MonsterBehaviour>();
    }
}
