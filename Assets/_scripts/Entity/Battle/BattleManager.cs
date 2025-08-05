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
    }
    private void Start()
    {
        _sm.ChangeState(BattleState.BattleStart);
        CheckWinLose();

    }
    public void Initialize()
    {
        deckManager.InitializeDeck();
        enemyManager.InitializeEnemies();
        playerController.Initialize();
    }
    public void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        _disposables?.Dispose();
    }
    public void CheckWinLose()
    {
        if (enemyManager.AllEnimiesDied())
        {
            _sm.ChangeState(BattleState.WinBattle);
        } else
        if (playerController.IsDead)
        {
            _sm.ChangeState(BattleState.LoseBattle);
        } else
        if (_sm.CurrentTypeState == BattleState.PlayerTurn)
        {
            _sm.ChangeState(BattleState.EnemyTurn);
        } 
        else if (_sm.CurrentTypeState != BattleState.EnemyTurn)
        {
            Debug.Log("eay eay");
            _sm.ChangeState(BattleState.PlayerTurn);
        }
    }
}