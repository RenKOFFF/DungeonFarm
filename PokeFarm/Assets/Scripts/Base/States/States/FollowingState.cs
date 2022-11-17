using UnityEngine;

public class FollowingState : BaseMonsterState
{
    private Transform _target;
    private float _socialDistance = 1.2f;
    private float _stopFollowingDistance = 3f;

    private float distanceToTarget;

    public State PreviousState { get; private set; }

    public FollowingState(Monster monster, float speed, GameObject target, State previousState)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
        _target = target.transform;

        PreviousState = previousState;
    }

    public override void Update()
    {
        MoveToTargetPosition();
    }
    private void MoveToTargetPosition()
    {
        distanceToTarget = Vector2.Distance(_monsterBehaviour.transform.position, _target.position);
        if (distanceToTarget >= _socialDistance)
        {
            _monsterBehaviour.transform.position = Vector2.MoveTowards(_monsterBehaviour.transform.position, _target.position, Time.deltaTime * _speed);
        }

        if (distanceToTarget > _stopFollowingDistance)
        {
            _monsterBehaviour.StateMachine.ChangeState(PreviousState);
        }
    }
}
