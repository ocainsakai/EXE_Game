using Ain;

public class PlayerDrawPhase : PlayerBaseState
{
    public PlayerDrawPhase(PlayerController playerHand) : base(playerHand)
    {
    }

    public override void OnEnter()
    {
        controller.handController.DrawCard(controller.deckList);
    }

    public override void OnExit()
    {
    }

    public  override void Tick()
    {
    }
}
