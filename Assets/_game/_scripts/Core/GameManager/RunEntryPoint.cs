using Map;
using VContainer;
using VContainer.Unity;

public class RunEntryPoint : IStartable
{
    private readonly IGameManager gameManager;
    private MapManager manager;
    [Inject]
    public RunEntryPoint(IGameManager gameManager, MapManager playerManager)
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
