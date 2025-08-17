using System.Collections.Generic;
public class StateMachine<TType,T> where T : IState
{
    protected Dictionary<TType, T> States;
    public IState CurrentState { get; private set; }
    public TType CurrentTypeState { get; private set; }
    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = newState;
        CurrentState.OnEnter();
    }
    public void ChangeState(TType newStateType)
    {
        CurrentTypeState = newStateType;
        if (States.TryGetValue(newStateType, out T newState))
        {
            ChangeState(newState);
        }
        else
        {
            throw new KeyNotFoundException($"State '{newStateType}' not found in the state machine.");
        }
    }
}