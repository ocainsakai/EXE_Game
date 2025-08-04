using Ain;
using UnityEngine;

public class PlayerDrawPhase : PlayerBaseState
{
    public PlayerDrawPhase(PlayerController playerHand) : base(playerHand)
    {
    }

    public override void OnEnter()
    {
         Debug.Log("PlayerDrawPhase: OnEnter");
        controller.handController.DrawCard(controller.deckList);
    }

    public override void OnExit()
    {
        Debug.Log("PlayerDrawPhase: OnExit");
        // Logic for exiting the draw phase, if any
        // For example, you might want to reset some state or update UI
    }

    public  override void Tick()
    {
    }
}
