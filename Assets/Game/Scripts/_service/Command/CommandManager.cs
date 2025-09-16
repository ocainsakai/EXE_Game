using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public IEntity Entity;
    public ICommand singleCommand;
    public List<ICommand> commands = new List<ICommand>();

    readonly CommandInvoke commandInvoke = new CommandInvoke();

    void Start()
    {
        Entity = GetComponent<IEntity>();
        // Execute a single command
        singleCommand = CommandBase.Create<AttackCommand>(Entity);
        //ExecuteCommand(singleCommand).Forget();
        // Execute a list of commands
        commands = new List<ICommand>
        {
            CommandBase.Create<AttackCommand>(Entity),
            CommandBase.Create<AttackCommand>(Entity)
        };
    }
    public async UniTask ExecuteCommand(List<ICommand> commands)
    {
        await commandInvoke.Excution(commands);
    }
}
