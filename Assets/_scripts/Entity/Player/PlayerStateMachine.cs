using Ain;

public class PlayerStateMachine : StateMachine<IState>
{
    public PlayerDrawPhase drawPhase;
    public PlayerAttackPhase attackPhase;
    public PlayerAction action;

    public void Initialiez(PlayerController controller, BattleManager battleManager)
    {
        //this.controller = controller;
        drawPhase = new PlayerDrawPhase(controller.handController, controller.deckList);
        attackPhase = new PlayerAttackPhase(controller, battleManager.enemyList);
        action = new PlayerAction(controller);
    }
}
