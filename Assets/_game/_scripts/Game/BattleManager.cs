using UnityEngine;
using TMPro;
using Map;
using System;


public class BattleManager : ManualSingleton<BattleManager>
{
    [SerializeField] private TextMeshProUGUI battleText;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TurnManager turnManager;
    private BattleState battleState;
    GameManager gameManager;
    //public BattleContext battleContext;

    [SerializeField] bool isTestMode = false;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        StartNewBattle();
    }
    private void OnEnable()
    {
        playerController.PlayerHealth.onDeath += GameLose;
        playerController.onPlayerEndTurn += PlayerEndHandle;
        enemyManager.Health.onDeath += GameWin;
        enemyManager.onEnemyEndTurn += EnemyEndHandle;
    }

    private void EnemyEndHandle()
    {
        // resolve
        Debug.Log("enemy end");
        turnManager.NextRound();
    }

    private void PlayerEndHandle()
    {
        enemyManager.Action();
    }

    private void OnDisable()
    {
        playerController.PlayerHealth.onDeath -= GameLose;
        playerController.onPlayerEndTurn -= PlayerEndHandle;
        enemyManager.Health.onDeath -= GameWin;
        enemyManager.onEnemyEndTurn -= EnemyEndHandle;
    }

    public void StartNewBattle()
    {

        // clear previous
        BattleClear();
        // load player
        playerController.LoadPlayerConfig(gameManager.playerConfig);
        // load enemies
        enemyManager.LoadEnemy(gameManager.enemies);
        // build deck
        playerController.BuidDeck();
        // init orther state
        battleState = new BattleState()
        {
            Player = playerController.playerState,
            Enemy = enemyManager.enemyState,
            TurnNumber = 0,
        };
        turnManager.SetTurnOnRound(2);
        turnManager.Initialze();
        turnManager.NextRound();
        //option save initial

    }

    public void BattleClear()
    {

    }
    private void GameWin()
    {
        // update ui and give rewards
        battleText.text = "Battle Win!";
        UIManager.Instance.OnWin();
    }

    private void GameLose()
    {
        // update ui and restart
        battleText.text = "Battle Lose!";
        UIManager.Instance.OnLose();
    }


}