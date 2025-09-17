using Cysharp.Threading.Tasks;

public abstract class CommandBase : ICommand
{
    protected readonly IEntity entity;
    protected CommandBase(IEntity entity)
    {
        this.entity = entity;
    }
    public abstract UniTask Execute();

    public static T Create<T>(IEntity entity) where T : CommandBase
    {
        return (T)System.Activator.CreateInstance(typeof(T), entity);
    }
}
