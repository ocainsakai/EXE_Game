using Ain;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class BattleStart : IState
{
    private readonly BattleStateMachine _battleManager;
    private CancellationTokenSource _cts;

    public BattleStart(BattleStateMachine battleManager)
    {
        _battleManager = battleManager;
    }
    public void OnEnter()
    {
        Debug.Log("Battle Start Enter");
        _cts = new CancellationTokenSource();
        //LoadCardsAsync(_cts.Token).Forget();
    }

    public void OnExit()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void Tick()
    {
        
    }
    //private async UniTaskVoid LoadCardsAsync(CancellationToken ct)
    //{
        
    //}
}
