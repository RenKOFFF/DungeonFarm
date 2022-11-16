using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PatrolState : BaseMonsterState
{
    private Transform[] _patrolPoints;
    private int _currentPatrolPointIndex;

    public PatrolState(Monster monster, float speed, PatrolData patrolData)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
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
            _monsterBehaviour.StateMachine.ChangeState(_monsterBehaviour.StateMachine.DefaultState);
            Debug.Log("Нет точек?");
        } 

        _monsterBehaviour.transform.position = Vector2.MoveTowards(
            _monsterBehaviour.transform.position, 
            _patrolPoints[_currentPatrolPointIndex].position, 
            _speed * Time.deltaTime);

        if (Vector2.Distance(_monsterBehaviour.transform.position, _patrolPoints[_currentPatrolPointIndex].position) <= .2f)
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
