using Ain;

public class PlayerAction : IState
{
    private PlayerController controller;
    public PlayerAction(PlayerController controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.EnableAllActions();
        // Logic for entering the action phase, if any
    }
    public void OnExit()
    {
        controller.DisableAllActions();
        // Logic for exiting the action phase, if any
    }
    public void Tick()
    {
        // Logic for updating the action phase, if any
    }
}