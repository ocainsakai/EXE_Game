using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>()
            .As<IGameManager>();
        builder.RegisterComponentInHierarchy<SceneLoader>()
            .As<ISceneLoader>();
        builder.RegisterComponentInHierarchy<PlayerData>();
        builder.RegisterEntryPoint<BootstrapEntryPoint>();
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); 
    }
}
