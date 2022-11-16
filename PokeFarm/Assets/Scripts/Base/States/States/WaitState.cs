using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : BaseMonsterState
{
    public WaitState(Monster monster, float speed)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
    }

    public override void Enter()
    {
        Debug.Log($"Enter {this}");
    }

    public override void Exit()
    {
        Debug.Log($"Exit {this}");
    }
}
