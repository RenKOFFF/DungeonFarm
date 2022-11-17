public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
    public virtual void Update() { }
}
