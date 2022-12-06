using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakRocksState : BaseMonsterCommandState
{
    public BreakRocksState(Monster monster, float speed, TileBase breakTile, Item workTool)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;

        _breakTile = breakTile;
        _workTool = workTool;
    }
}