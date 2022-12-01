using UnityEngine;

public abstract class BaseMonsterState : State
{
    protected MonsterBehaviour _monsterBehaviour;
    protected float _speed;

    protected float _waitTime;
    protected float _startWaitTime = 2f;

    public override void Enter()
    {
        Debug.Log($"Enter {this}");
    }

    public override void Exit()
    {
        Debug.Log($"Exit {this}");
    }
}