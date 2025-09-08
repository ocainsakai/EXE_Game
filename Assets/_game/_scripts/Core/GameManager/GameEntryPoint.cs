using VContainer;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    private readonly IGameManager gameManager;

    [Inject]
    public GameEntryPoint(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Start()
    {
        gameManager.Init();
    }
}
