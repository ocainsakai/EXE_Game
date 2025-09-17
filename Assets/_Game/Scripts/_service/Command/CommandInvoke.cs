using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class CommandInvoke { 
    public async UniTask Excution(List<ICommand> commands) 
    {
        foreach(var command in commands)
        {

            await command.Execute();
        }
    }   
}
