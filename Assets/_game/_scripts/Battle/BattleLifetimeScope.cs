using VContainer;
using VContainer.Unity;

public class BattleLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<BattleManager>();
        builder.RegisterComponentInHierarchy<BattlePlayer>();
        builder.RegisterComponentInHierarchy<BattleEnemy>();
        builder.RegisterComponentInHierarchy<TurnManager>();
    }
}
