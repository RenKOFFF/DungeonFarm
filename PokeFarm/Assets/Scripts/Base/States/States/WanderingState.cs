using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingState : State
{
    private Monster _monster;
    private float _speed;

    private float _waitTime;
    private float _startWaitTime = 2f;

    //точка от которой чача будет перемещаться на расстояние не больше чем maxWanderingDistance
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
        _centerWanderingArea = _monster.transform.position;
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
        var findStopCount = 100;
        while (findStopCount > 0)
        {
            var rndDistance = Random.Range(0, maxWanderingDistance);

            //рандомное движение в точку на окружности
            var randAng = Random.Range(0, Mathf.PI * 2);
            _movePoint = _centerWanderingArea +
                         new Vector2(Mathf.Cos(randAng) * rndDistance, Mathf.Sin(randAng) * rndDistance);
            
            findStopCount--;
            
            if (/*LandscapeController.Instance.prefabsTilema*/
                true) break;
        }
    }

    public override void Exit()
    {
        Debug.Log($"Exit {this}");
    }

    public override void Update()
    {
        MoveTo(_movePoint);
    }
    private void MoveTo(Vector2 movePoint)
    {
        _monster.transform.position = Vector2.MoveTowards(_monster.transform.position,
            movePoint, 
            Time.deltaTime * _speed);

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
