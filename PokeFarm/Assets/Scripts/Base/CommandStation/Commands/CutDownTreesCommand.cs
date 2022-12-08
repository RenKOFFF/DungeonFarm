using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.CommandStation.Commands
{
    public class CutDownTreesCommand : Command, ICommand
    { 
        [SerializeField] private TileBase _breakTile;
        [SerializeField] private Item _workTool;
        [SerializeField] private int _commandEnergyCost = 3;

        public override void Execute()
        {
            CurrentMonster.MonsterBehaviour.GiveCommand(
                new CutDownTreesState(CurrentMonster, _breakTile, _workTool, _commandEnergyCost));
        }
    }
}