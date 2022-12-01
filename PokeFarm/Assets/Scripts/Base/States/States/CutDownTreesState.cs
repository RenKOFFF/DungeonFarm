using UnityEngine;

public class CutDownTreesState : BaseMonsterState
{
    private Vector3Int _currentTreePosition;
    public CutDownTreesState(Monster monster, float speed)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;
    }

    public override void Enter()
    {
        base.Enter();
        if (LandscapeController.Instance.SpawnableTilesList.Count > 0)
            _currentTreePosition = LandscapeController.Instance.SpawnableTilesList[0];
    }

    public override void Update()
    {
        MoveTo(_currentTreePosition);
    }
    private void MoveTo(Vector3Int movePoint)
    {
        _monsterBehaviour.Monster.transform.position = Vector3.MoveTowards
            (_monsterBehaviour.Monster.transform.position, 
                movePoint, 
                Time.deltaTime * _speed);
    }
}