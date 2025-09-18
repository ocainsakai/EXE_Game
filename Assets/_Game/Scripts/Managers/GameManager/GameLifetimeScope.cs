using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlayerData>();
        //builder.RegisterComponentInHierarchy<PlayerData>();
        //builder.RegisterComponentInHierarchy<UIManager>();
        builder.RegisterEntryPoint<BootstrapEntryPoint>();
    }
}
