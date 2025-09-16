using System;
using System.Threading.Tasks;

public abstract class Command<TCommand> : ICommand
    where TCommand : Command<TCommand>
{
    public abstract string CommandName { get; }

    protected Command()
    {
        if (this is not TCommand)
        {
            throw new InvalidOperationException($"Invalid command type: {typeof(TCommand).Name}");
        }
    }

    public Task ExecuteWithAsync(ICommandDispatcher dispatcher)
        => dispatcher.ExecuteAsync((TCommand)this);

    public Task<bool> ValidateWithAsync(ICommandDispatcher dispatcher)
        => dispatcher.ValidateAsync((TCommand)this);

    public string SerializeWith(ICommandSerializer serializer)
        => serializer.Serialize((TCommand)this);
}