using Ain;

using System.Threading;
using UnityEngine;

public class BattleStart : IState
{
    private readonly BattleManager _battleManager;
    private CardList deckList;
    private CancellationTokenSource _cts;

    public BattleStart(BattleManager battleManager, CardList deckList)
    {
        _battleManager = battleManager;
        this.deckList = deckList;
    }
    public void OnEnter()
    {
        _battleManager.InitializeDeck();
        _battleManager.InitializeEnemies();
        _battleManager.InitializePlayer();
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
