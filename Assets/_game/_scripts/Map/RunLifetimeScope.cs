using Map;
using VContainer;
using VContainer.Unity;

public class RunLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlayerManager>();
        builder.RegisterComponentInHierarchy<BattleManager>();
        builder.RegisterComponentInHierarchy<EnemyManager>();
        builder.RegisterComponentInHierarchy<MapManager>();
        builder.RegisterComponentInHierarchy<RunManager>();
        builder.RegisterEntryPoint<RunEntryPoint>();
    }
}
