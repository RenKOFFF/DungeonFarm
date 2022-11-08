using Unity.VisualScripting;
using UnityEngine;

public class FollowingState : State
{
    private MonsterBehaviour _monsterBehaviour;
    private float _speed;

    private float _waitTime;
    private float _startWaitTime = 2f;

    private Transform _target;
    private float _socialDistance = 1.2f;
    private float _stopFollowingDistance = 3f;

    public FollowingState(Monster monster, float speed, GameObject target)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
        _target = target.transform;
    }

    public override void Enter()
    {
        Debug.Log($"Enter {this}");
    }

    public override void Exit()
    {
        Debug.Log($"Exit {this}");
    }

    public override void Update()
    {
        MoveToTargetPosition();
    }
    private void MoveToTargetPosition()
    {
        if (Vector2.Distance(_monsterBehaviour.transform.position, _target.position) >= _socialDistance)
        {
            _monsterBehaviour.transform.position = Vector2.MoveTowards(_monsterBehaviour.transform.position, _target.position, Time.deltaTime * _speed);
        }

        if (Vector2.Distance(_monsterBehaviour.transform.position, _target.position) > _stopFollowingDistance)
        {
            _monsterBehaviour.StateMachine.ChangeState(_monsterBehaviour.StateMachine.DefaultState);
        }
    }
}
