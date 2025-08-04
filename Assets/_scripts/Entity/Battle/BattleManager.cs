using UnityEngine;
using System;
using UniRx;

public class BattleManager : Singleton<BattleManager>, IDisposable
{
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public DeckManager deckManager;
    [SerializeField] public PlayerController playerController;
    
    private BattleStateMachine _sm;
    private CompositeDisposable _disposables = new CompositeDisposable();
    protected override void Awake()
    {
        base.Awake();
        _sm = new BattleStateMachine(this);
        playerController.OnPlayerEndTurn += EnemyTurn;
    }
    private void Start()
    {
        _sm.ChangeState(BattleState.BattleStart);
        PlayerTurn();

    }
    public void Initialize()
    {
        deckManager.InitializeDeck();
        enemyManager.InitializeEnemies();
        playerController.Initialize();
    }
    public void PlayerTurn()
    {
        _sm.ChangeState(BattleState.PlayerTurn);
    }
    public void WinBattle() =>
            _sm.ChangeState(BattleState.WinBattle);
    public void LoseBattle() =>
            _sm.ChangeState(BattleState.LoseBattle);
    public void EnemyTurn() =>    
        _sm.ChangeState(BattleState.EnemyTurn);


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