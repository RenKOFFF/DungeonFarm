using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakeRocksCommand : Command, ICommand
{
    [SerializeField] private TileBase _breakTile; 
    [SerializeField] private Item _workTool;
    public override void Execute()
    {
        CurrentMonster.MonsterBehaviour.StateMachine.ChangeState(
            new BreakRocksState(CurrentMonster, CurrentMonster.Speed, _breakTile, _workTool));
    }
}