using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakeRocksCommand : Command, ICommand
{
    [SerializeField] private TileBase _breakTile; 
    public override void Execute()
    {
        CurrentMonster.MonsterBehaviour.StateMachine.ChangeState(
            new BreakRocksState(CurrentMonster, CurrentMonster.Speed, _breakTile));
    }
}