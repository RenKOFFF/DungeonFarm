using UnityEngine;

namespace Base.CommandStation.Commands
{
    [CreateAssetMenu(fileName = "Command", menuName = "CommandStation/Command", order = 0)]
    public class CommandDataSO : ScriptableObject
    {
        public Sprite Icon { get => _icon; private set => _icon = value; }
        public string CommandName { get => commandName; private set => commandName = value; }

        [SerializeField] private Sprite _icon;
        [SerializeField] private string commandName;
        [SerializeField] public Command Command;
    }
}