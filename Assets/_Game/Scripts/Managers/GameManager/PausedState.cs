using Game.Service;

public class PausedState : IState
{
    public void OnEnter()
    {
        // TODO: Freeze game, hiện menu pause
    }

    public void Update()
    {
        // TODO: Xử lý input trong Pause menu
    }

    public void FixedUpdate()
    {
        // TODO: Pause thường không có FixedUpdate
    }

    public void OnExit()
    {
        // TODO: Resume game
    }
}
