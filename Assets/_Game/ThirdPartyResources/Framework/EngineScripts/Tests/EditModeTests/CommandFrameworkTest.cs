using System.Threading.Tasks;
using NUnit.Framework;

public class CommandFrameworkTest
{
    private class State
    {
        public string Value { get; set; } = "";
    }

    private class TestCommand : Command<TestCommand>
    {
        public override string CommandName => "BoardGame::Tests::Test";

        public string Message { get; }

        public State State { get; }

        public TestCommand(string message, State state)
        {
            Message = message;
            State = state;
        }
    }

    private class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task ExecuteAsync(TestCommand command)
        {
            command.State.Value = command.Message;
            return Task.CompletedTask;
        }

        public Task<bool> ValidateAsync(TestCommand command)
        {
            return Task.FromResult(true);
        }
    }

    private ICommandManager _commandManager;

    [SetUp]
    public void SetUp()
    {
        var commandDispatcher = new CommandDispatcher();
        commandDispatcher.Register(new TestCommandHandler());
        _commandManager = new CommandManager(
            commandDispatcher,
            new InMemoryCommandRecorder<ICommand>()
        );
    }

    [Test]
    public async Task TestCommandExecution()
    {
        var state = new State();

        var cmd = new TestCommand("Test message", state);

        Assert.AreEqual("", state.Value);

        await _commandManager.ExecuteAsync(cmd);

        Assert.AreEqual("Test message", state.Value);

        var cmd2 = new TestCommand("Test message 2", state);

        await _commandManager.ExecuteAsync(cmd2);

        Assert.AreEqual("Test message 2", state.Value);
    }
}