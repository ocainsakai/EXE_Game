using System.Collections.Generic;
public class PlayerStateMachine : StateMachine<PlayerStateType, PlayerBaseState>
{
    public PlayerStateMachine(PlayerController controller)
    {
        States = new Dictionary<PlayerStateType, PlayerBaseState>
        {
            { PlayerStateType.Idle, new PlayerIdleState(controller) },
            { PlayerStateType.Action, new PlayerAction(controller) },
            { PlayerStateType.Attack, new PlayerAttackPhase(controller) },
            { PlayerStateType.Draw, new PlayerDrawPhase(controller) }
        };

        ChangeState(PlayerStateType.Idle);
    }
}
