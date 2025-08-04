using Ain;
using System.Threading;
using UnityEngine;

public class BattleStart : IState
{
    private readonly BattleManager _battleManager;
    private CancellationTokenSource _cts;

    public BattleStart(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }
    public void OnEnter()
    {
        _battleManager.Initialize();
        _cts = new CancellationTokenSource();

    }

    public void OnExit()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void Tick()
    {
        
    }
   
}
