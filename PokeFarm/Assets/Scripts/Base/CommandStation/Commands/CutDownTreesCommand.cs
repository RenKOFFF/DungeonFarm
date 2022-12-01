using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.CommandStation.Commands
{
    public class CutDownTreesCommand : Command, ICommand
    { 
        [SerializeField] private TileBase _breakTile; 
        public override void Execute()
        {
            CurrentMonster.MonsterBehaviour.StateMachine.ChangeState(
                new CutDownTreesState(CurrentMonster, CurrentMonster.Speed, _breakTile));
        }
    }
}