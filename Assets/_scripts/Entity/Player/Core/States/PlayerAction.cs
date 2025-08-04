
public class PlayerAction : PlayerBaseState
{
    public PlayerAction(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        controller.EnableAllActions();
        // Logic for entering the action phase, if any
    }
    public override void OnExit()
    {
        controller.DisableAllActions();
        // Logic for exiting the action phase, if any
    }

    public override void Tick()
    {
       
    }
}