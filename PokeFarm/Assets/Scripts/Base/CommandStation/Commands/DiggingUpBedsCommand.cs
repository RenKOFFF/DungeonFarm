using Base.States.States;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.CommandStation.Commands
{
    public class DiggingUpBedsCommand : Command, ICommand
    {
        [SerializeField] private PlantingCycleTile _diggingUpPlantingCycleTile;
        [SerializeField] private Item _workTool;
        [SerializeField] private int _commandEnergyCost = 1;

        public override void Execute()
        {
            CurrentMonster.MonsterBehaviour.GiveCommand(
                new DiggingUpBedsState(CurrentMonster, _diggingUpPlantingCycleTile, _workTool, _commandEnergyCost));
        }
    }
}