using Ain;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] public CardList deckList;
    [SerializeField] public EnemyList enemyList;
    [SerializeField] public CardDatabase standardCards;
    [SerializeField] public EnemyDatabase combatContext;
    [SerializeField] public EnemyBattleUI battleUI;
    [SerializeField] private BattleStateMachine battleStateMachine;

    protected override void Awake()
    {
        battleStateMachine.Initialize(this);
        base.Awake();
        //battleStateMachine = new BattleStateMachine(this);
    }
    private void Start()
    {
        //SetInitialState(new BattleStart(this, deckList));
        InitializeEnemies();
    }

    private void InitializeEnemies()
    {
        var enemies = battleUI.InitializedEnemy(combatContext.Enemies);
        enemyList.enemies.Clear();
        enemyList.enemies.AddRange(enemies);
    }

    public List<Card> CreateStandardCard()
    {
        return CardFactory.Instance.GetCards(standardCards.Cards);
    }
}