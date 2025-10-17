using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public IEntity Entity;
    public ICommand SingleCommand;
    public List<ICommand> Commands = new List<ICommand>();

    readonly CommandInvoke commandInvoke = new CommandInvoke();

    void Start()
    {
        Entity = GetComponent<IEntity>();
        // Execute a single command
        SingleCommand = CommandBase.Create<AttackCommand>(Entity);
        //ExecuteCommand(singleCommand).Forget();
        // Execute a list of commands
        Commands = new List<ICommand>
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
