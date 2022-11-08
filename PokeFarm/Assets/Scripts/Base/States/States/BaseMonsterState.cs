using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonsterState : State
{
    protected MonsterBehaviour _monsterBehaviour;
    protected float _speed;

    protected float _waitTime;
    protected float _startWaitTime = 2f;
}
