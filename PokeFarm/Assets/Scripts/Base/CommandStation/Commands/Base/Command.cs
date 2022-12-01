using UnityEngine;

namespace Base.CommandStation.Commands
{
    public abstract class Command : MonoBehaviour, ICommand
    {
        public CommandDataSO CommandDataSO;
        public Monster CurrentMonster { get; set; }
        public abstract void Execute();
    }
}