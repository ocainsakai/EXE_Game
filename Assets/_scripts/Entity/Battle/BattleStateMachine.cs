using Ain;
using UnityEngine;

public class BattleStateMachine : StateMachine<IState>
{
    public Player player;
    private void Start()
    {
        SetInitialState(new BattleStart(this));
    }
    public void Update()
    {
        CurrentState?.Tick();
    }
    public void Draw()
    {
        //ChangeState(new DrawPhase(player));
    }
}