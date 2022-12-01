using UnityEngine;

public class BreakeRocksState : BaseMonsterState
{
    private Vector2 _currentRockPosition;
    public BreakeRocksState(Monster monster, float speed)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
    }
    public override void Update()
    {
        MoveTo(_currentRockPosition);
    }
    private void MoveTo(Vector2 movePoint)
    {
        _monsterBehaviour.Monster.transform.position = Vector2.MoveTowards
        (_monsterBehaviour.Monster.transform.position, 
            movePoint, 
            Time.deltaTime * _speed);
    }
}