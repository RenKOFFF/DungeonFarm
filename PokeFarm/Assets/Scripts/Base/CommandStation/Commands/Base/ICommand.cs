using UnityEditor.Sprites;

public interface ICommand
{
    Monster CurrentMonster { get; set; }
    void Execute();
}