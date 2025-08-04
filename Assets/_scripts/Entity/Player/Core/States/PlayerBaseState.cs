using Ain;

public abstract class PlayerBaseState : IState
{
    protected PlayerController controller;
    public PlayerBaseState(PlayerController controller)
    {
        this.controller = controller;
    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Tick();
}
