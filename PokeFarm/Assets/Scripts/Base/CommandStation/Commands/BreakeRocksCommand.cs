using System.Collections;
using System.Collections.Generic;
using Base.CommandStation.Commands;
using UnityEngine;

public class BreakeRocksCommand : Command, ICommand
{ 
    public override void Execute()
    {
        CurrentMonster.MonsterBehaviour.StateMachine.ChangeState(new BreakeRocksState(CurrentMonster, CurrentMonster.Speed));
    }
}