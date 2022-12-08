using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakeRocksCommand : Command, ICommand
{
    [SerializeField] private TileBase _breakTile; 
    [SerializeField] private Item _workTool;
    [SerializeField] private int _commandEnergyCost = 5;
    public override void Execute()
    {
        //TODO сделать вариант с отказом от работы
        CurrentMonster.MonsterBehaviour.GiveCommand(
            new BreakRocksState(CurrentMonster, _breakTile, _workTool, _commandEnergyCost));
    }
}