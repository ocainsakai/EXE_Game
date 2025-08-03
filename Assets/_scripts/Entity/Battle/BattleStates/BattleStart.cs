using Ain;

using System.Threading;
using UnityEngine;

public class BattleStart : IState
{
    private readonly BattleStateMachine _battleManager;
    private CardList deckList;
    private CancellationTokenSource _cts;

    public BattleStart(BattleStateMachine battleManager, CardList deckList)
    {
        _battleManager = battleManager;
        this.deckList = deckList;
    }
    public void OnEnter()
    {
        deckList.cards.Clear();
        Debug.Log("Battle Start Enter");
        //deckList.cards = _battleManager.CreateStandardCard();
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
    //private async UniTaskVoid LoadCardsAsync(CancellationToken ct)
    //{
        
    //}
}
