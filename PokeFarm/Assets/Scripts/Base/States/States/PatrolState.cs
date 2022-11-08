using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PatrolState : State
{
    private MonsterBehaviour _monster;
    private float _speed;

    private Transform[] _patrolPoints;
    private int _currentPatrolPointIndex;

    private float _waitTime;
    private float _startWaitTime = 2f;

    public PatrolState(Monster monster, float speed, PatrolData patrolData)
    {
        _monster = monster.MonsterBehaviour;
        _speed = speed;
        _patrolPoints = patrolData.PatrolPoints;

        _waitTime = _startWaitTime;
    }

    public override void Enter()
    {
        Debug.Log("Enter PATROL");
    }

    public override void Exit()
    {
        Debug.Log("Exit PATROL");
    }

    public override void Update()
    {
        if (_patrolPoints == null && _patrolPoints.Length == 0)
        {
            _monster.StateMachine.ChangeState(_monster.StateMachine.DefaultState);
            Debug.Log("Нет точек?");
        } 

        _monster.transform.position = Vector2.MoveTowards(
            _monster.transform.position, 
            _patrolPoints[_currentPatrolPointIndex].position, 
            _speed * Time.deltaTime);

        if (Vector2.Distance(_monster.transform.position, _patrolPoints[_currentPatrolPointIndex].position) <= .2f)
        {
            if (_waitTime <= 0)
            {
                _currentPatrolPointIndex++;
                _waitTime = _startWaitTime;
            }
            else _waitTime -= Time.deltaTime;
        }

        if (_currentPatrolPointIndex > _patrolPoints.Length - 1) _currentPatrolPointIndex = 0;

        
    }
}
