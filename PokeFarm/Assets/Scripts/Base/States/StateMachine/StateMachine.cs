public class StateMachine
{
    public State CurrentState { get; private set; }
    public State DefaultState { get; private set; }
    public void Init(State startState)
    {
        DefaultState = startState;
        if(CurrentState == null) 
            ChangeState(DefaultState);
    }

    public void ChangeState(State newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();

    }
}
