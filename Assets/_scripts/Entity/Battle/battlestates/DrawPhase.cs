using Ain;
using UnityEngine;

public class DrawPhase : IState
{
    private readonly BattleStateMachine _stateMachine;
    public DrawPhase(BattleStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void OnEnter()
    {
        //CardFactory.Instance.SpawnCardPrf();
        Debug.Log("Draw Phase Enter");
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}
