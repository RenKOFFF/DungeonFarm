using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CutDownTreesState : BaseMonsterCommandState
{
    public CutDownTreesState(Monster monster, TileBase breakTile, Item workTool, int commandEnergyCost)
    {
        _monsterBehaviour = monster.MonsterBehaviour;
        _speed = monster.Stats.Speed;

        _waitTime = _startWaitTime;
        _breakTile = breakTile;
        _workTool = workTool;
        CommandEnergyCost = commandEnergyCost;
    }
}