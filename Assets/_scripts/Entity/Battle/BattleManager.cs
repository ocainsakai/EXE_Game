using UnityEngine;
using System;
using UniRx;

public class BattleManager : MonoBehaviour, IDisposable
{
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public DeckManager deckManager;
    [SerializeField] public PlayerController playerController;
    
    private BattleStateMachine _sm;
    private CompositeDisposable _disposables = new CompositeDisposable();
    protected void Awake()
    {
        _sm = new BattleStateMachine(this);
        playerController.OnPlayerEndTurn += CheckCondition;
    }
    private void Start()
    {
        _sm.ChangeState(BattleState.BattleStart);
    }
    public void Initialize()
    {
        deckManager.InitializeDeck();
        enemyManager.InitializeEnemies();
        playerController.Initialize(this);
    }
    public void PlayerTurn()
    {
        _sm.ChangeState(BattleState.PlayerTurn);
    }
    public void CheckCondition()
    {
        Debug.Log("Checking battle conditions...");
        if (enemyManager.AllEnimiesDied())
        {
            _sm.ChangeState(BattleState.WinBattle);
        }
        else 
        {
            _sm.ChangeState(BattleState.LoseBattle);
        }
    }

    public void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        _disposables?.Dispose();
    }

    internal void ClearBattle()
    {
    }
}