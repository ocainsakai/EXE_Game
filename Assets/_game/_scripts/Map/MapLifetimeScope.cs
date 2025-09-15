using Map;
using VContainer;
using VContainer.Unity;

public class MapLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MapGrid>();
        builder.RegisterComponentInHierarchy<MapPopup>();
        builder.RegisterComponentInHierarchy<MapUI>();
        builder.RegisterComponentInHierarchy<MapMoving>();
        builder.RegisterComponentInHierarchy<MapManager>();
    }
}
