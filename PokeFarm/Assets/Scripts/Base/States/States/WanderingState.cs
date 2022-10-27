using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public class WanderingState : State
{
    private Monster _monster;
    private float _speed;

    private float _waitTime;
    private float _startWaitTime = 2f;

    //����� �� ������� ���� ����� ������������ �� ���������� �� ������ ��� maxWanderingDistance
    private Vector2 _centerWanderingArea;
    private Vector2 _movePoint;
    private float _maxWanderingDistance = 3f;

    private const float ARRIVAL_TO_POINT_DISTANCE = 1f;

    public WanderingState(Monster monster, float speed, float maxWanderingDistance)
    {
        _monster = monster;
        _speed = speed;

        _waitTime = _startWaitTime;

        _maxWanderingDistance = maxWanderingDistance;
    }

    public WanderingState(Monster monster, float speed, float maxWanderingDistance, Vector2 centerWanderingArea) : 
        this(monster, speed, maxWanderingDistance)
    {
        _centerWanderingArea = centerWanderingArea;
    }

    public override void Enter()
    {
        Debug.Log($"Enter {this}");
        FindNewPoint(_maxWanderingDistance);
    }

    private void FindNewPoint(float maxWanderingDistance)
    {
        var rndDistance = Random.Range(0, maxWanderingDistance);

        //��������� �������� � ����� �� ����������
        var randAng = Random.Range(0, Mathf.PI * 2);

        if (_centerWanderingArea != null)
        {
            _movePoint = _centerWanderingArea + new Vector2(Mathf.Cos(randAng) * rndDistance, Mathf.Sin(randAng) * rndDistance);
        }
        else
        {
            _movePoint = (Vector2)_monster.transform.position + new Vector2(Mathf.Cos(randAng) * rndDistance, Mathf.Sin(randAng) * rndDistance);
        }
    }

    public override void Exit()
    {
        Debug.Log($"Exit {this}");
    }

    public override void Update()
    {
        //Gizmos.DrawLine(_monster.transform.position, _movePoint);

        MoveTo(_movePoint);
    }
    private void MoveTo(Vector3 movePoint)
    {
        _monster.transform.position = Vector3.MoveTowards(_monster.transform.position, movePoint, Time.deltaTime * _speed);

        if (Vector2.Distance(_monster.transform.position, movePoint) <= ARRIVAL_TO_POINT_DISTANCE)
        {
            if (_waitTime <= 0)
            {
                FindNewPoint(_maxWanderingDistance);
                _waitTime = _startWaitTime;
            }
            else _waitTime -= Time.deltaTime;
        }
    }
}
