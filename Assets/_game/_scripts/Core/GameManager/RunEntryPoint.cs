using VContainer;
using VContainer.Unity;

public class RunEntryPoint : IStartable
{
    private readonly IGameManager gameManager;
    private RunManager manager;
    [Inject]
    public RunEntryPoint(IGameManager gameManager, RunManager playerManager)
    {
        this.gameManager = gameManager;
        this.manager = playerManager;
    }

    public void Start()
    {
        gameManager.Init();
        manager.InitNew();
    }
}
