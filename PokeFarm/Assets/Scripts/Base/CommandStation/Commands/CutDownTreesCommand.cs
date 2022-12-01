using UnityEngine;

namespace Base.CommandStation.Commands
{
    public class CutDownTreesCommand : Command, ICommand
    { 
        public override void Execute()
        {
            CurrentMonster.MonsterBehaviour.StateMachine.ChangeState(new CutDownTreesState(CurrentMonster, CurrentMonster.Speed));
        }
    }
}