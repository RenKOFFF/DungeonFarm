using UnityEngine;

public class FollowingState : State
{
    private Monster _monster;
    private float _speed;

    private float _waitTime;
    private float _startWaitTime = 2f;

    private Transform _target;
    private float _socialDistance = 1.2f;
    private float _stopFollowingDistance = 3f;

    public FollowingState(Monster monster, float speed, GameObject target)
    {
        _monster = monster;
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
        if (Vector2.Distance(_monster.transform.position, _target.position) >= _socialDistance)
        {
            _monster.transform.position = Vector2.MoveTowards(_monster.transform.position, _target.position, Time.deltaTime * _speed);
        }

        if (Vector2.Distance(_monster.transform.position, _target.position) > _stopFollowingDistance)
        {
            _monster.StateMachine.ChangeState(_monster.StateMachine.DefaultState);
        }
    }
}
