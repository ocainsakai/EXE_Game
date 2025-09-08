using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private UILoading loading;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>()
            .As<IGameManager>();
        builder.RegisterComponentInHierarchy<SceneLoader>()
            .As<ISceneLoader>();
        builder.RegisterComponentInHierarchy<TransitionDataService>()
            .As<ITransitionDataService>();
        builder.RegisterComponentInHierarchy<PlayerData>();
        builder.RegisterEntryPoint<BootstrapEntryPoint>().WithParameter("loadingUI", loading);
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); 
    }
}
