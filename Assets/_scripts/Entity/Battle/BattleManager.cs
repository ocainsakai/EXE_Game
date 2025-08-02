using Ain;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public enum BattleState { PlayerTurn, EnemyTurn, Victory, Defeat }

public class BattleManager : Singleton<BattleManager>
{
    public CancellationTokenSource CancellationTokenSource { get; private set; }
    public IState CurrentState { get; private set; }
    public void ChangeState(IState state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = state;
        if (CurrentState != null)
        {
            CurrentState.OnEnter();
        }
    }
    private void OnDestroy()
    {
        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
    }
  
}
