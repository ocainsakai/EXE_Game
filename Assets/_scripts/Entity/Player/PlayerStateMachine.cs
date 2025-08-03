using Ain;

public class PlayerStateMachine : StateMachine<IState>
{
    public PlayerDrawPhase drawPhase;
    public PlayerAttackPhase attackPhase;
    private PlayerController controller;
    public void Initialiez(PlayerController controller)
    {
        this.controller = controller;
        drawPhase = new PlayerDrawPhase(controller.handController, controller.deckList);
        attackPhase = new PlayerAttackPhase(controller.handController, controller.enemyList);
    }
}
