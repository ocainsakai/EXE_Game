using Ain;

public class EnemyCounting : IState
{
    private Counter _counter;

    public EnemyCounting(Counter counter)
    {
        _counter = counter;
    }

    public void OnEnter()
    {
        _counter.DecreaseCount();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}

