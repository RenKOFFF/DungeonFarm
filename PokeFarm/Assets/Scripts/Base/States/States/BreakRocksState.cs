using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakRocksState : BaseMonsterCommandState
{
    public BreakRocksState(Monster monster, float speed, TileBase breakTile)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = speed;

        _waitTime = _startWaitTime;

        _breakTile = breakTile;
    }
}