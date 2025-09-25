using Cysharp.Threading.Tasks;
using UnityEngine;

public class AttackCommand : CommandBase
{
    public AttackCommand(IEntity entity) : base(entity)
    {
    }
    public override async UniTask Execute()
    {
        Debug.Log("AttackCommand Execute");
        await UniTask.Delay(1000);
    }
}