using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : State
{
    private Monster _monster;
    private Transform _restPlace;
    private float Speed;

    private float _waitToSleepTime;
    private float _startWaitToSleepTime = 5f;

    public RestState(Monster monster, float speed, Transform restPlace)
    {
        _monster = monster;
        _restPlace = restPlace;
        Speed = speed;

        _waitToSleepTime = _startWaitToSleepTime;
    }
    public override void Enter()
    {
        Debug.Log("Enter IDLE");
    }

    public override void Exit()
    {
        Debug.Log("Exit IDLE");
    }

    public override void Update()
    {
        if (Vector2.Distance(_monster.transform.position, _restPlace.position) <= .2f)
        {
            if (_waitToSleepTime < -7f)
            {
                _waitToSleepTime = _startWaitToSleepTime;
                Debug.Log("Wake up...");
                _monster.StateMachine.ChangeState(_monster.RatrolState);
            }
            else if (_waitToSleepTime <= 0)
            {
                Debug.Log("Sleeping...Zzz ");
                _waitToSleepTime -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Resting...");
                _waitToSleepTime -= Time.deltaTime;
            }
        }
        _monster.transform.position = Vector2.MoveTowards(
           _monster.transform.position,
           _restPlace.position,
           Speed * Time.deltaTime);
    }
}
