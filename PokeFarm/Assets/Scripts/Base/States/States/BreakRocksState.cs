using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakRocksState : BaseMonsterCommandState
{
    public BreakRocksState(Monster monster, TileBase breakTile, Item workTool, int _commandEnergyCost)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = monster.Stats.Speed;

        _waitTime = _startWaitTime;

        _breakTile = breakTile;
        _workTool = workTool;
        CommandEnergyCost = _commandEnergyCost;
    }
}