using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UILoading loading;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<ISceneLoader>(sceneLoader);
        builder.RegisterComponent<IGameManager>(gameManager);

        builder.RegisterEntryPoint<BootstrapEntryPoint>().WithParameter("loadingUI", loading);
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); 
    }
}
