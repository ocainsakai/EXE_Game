using Ain;
using UnityEngine;
using System;
using UniRx;

public class BattleManager : MonoBehaviour, IDisposable
{
    
    [SerializeField] public CardList deckList;
    [SerializeField] public EnemyList enemyList;
    [SerializeField] public CardDatabase standardCards;
    [SerializeField] public EnemyDatabase combatContext;
    [SerializeField] public EnemyBattleUI battleUI;
    [SerializeField] public PlayerController playerController;
    [SerializeField] private BattleStateMachine _sm;

    private CompositeDisposable _disposables = new CompositeDisposable();
    protected void Awake()
    {
        _sm.Initialize(this, playerController);
        //base.Awake();
    }
    private void Start()
    {
        _sm.ChangeState(_sm.battleStart);
    }
    public void InitializeDeck()
    {
        var cards = CardFactory.Instance.GetCards(standardCards.Cards);
        deckList.cards.Clear();
        deckList.cards.AddRange(cards);
    }
    public void InitializeEnemies()
    {
        var enemies = battleUI.InitializedEnemy(combatContext.Enemies);
        enemyList.enemies.Clear();
        enemyList.enemies.AddRange(enemies);
    }
    public void InitializePlayer()
    {
        playerController.Initialize(this);
    }
    public void PlayerTurn()
    {
        _sm.ChangeState(_sm.playerTurn);
    }
    public void CheckCondition()
    {
        if (enemyList.enemies.TrueForAll(x => x.IsDead.Value))
        {
            _sm.ChangeState(_sm.winBattle);
        }
        else 
        {
            _sm.ChangeState(_sm.enemyTurn);
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