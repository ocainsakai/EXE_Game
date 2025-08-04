using UnityEngine;

internal class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("PlayerIdleState: OnEnter");
        // Logic for entering the idle state, if any
        // For example, you might want to reset some UI elements or player state
        //controller.handController.ResetHand();
        //controller.handController.DisableAllActions();
    }

    public override void OnExit()
    {
        Debug.Log("PlayerIdleState: OnExit");
        // Logic for exiting the idle state, if any
        // For example, you might want to enable some UI elements or player actions
        //controller.handController.EnableAllActions();
    }

    public override void Tick()
    {
    }
}