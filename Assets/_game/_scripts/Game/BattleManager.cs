using Map;
using System;
using TMPro;
using UnityEngine;
using UnityUtils;

public class BattleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI battleText;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TurnManager turnManager;

    private GameManager gameManager;

    [SerializeField] private bool isTestMode = false;

    public event Action OnBattleWin;
    public event Action OnBattleLose;

    private void Start()
    {
        gameManager = GameManager.Instance;
        StartNewBattle();
    }

    private void OnEnable()
    {
        playerController.PlayerHealth.onDeath += HandleGameLose;
        playerController.onPlayerEndTurn += HandlePlayerEndTurn;

        enemyManager.Health.onDeath += HandleGameWin;
        enemyManager.onEnemyEndTurn += HandleEnemyEndTurn;
    }

    private void OnDisable()
    {
        playerController.PlayerHealth.onDeath -= HandleGameLose;
        playerController.onPlayerEndTurn -= HandlePlayerEndTurn;

        enemyManager.Health.onDeath -= HandleGameWin;
        enemyManager.onEnemyEndTurn -= HandleEnemyEndTurn;
    }
    public void Clamp()
    {
        GameManager.Instance.ChangeScenceToMap();
    }

    #region Battle Lifecycle
    public void StartNewBattle()
    {
        //UIManager.Instance.CloseAll();


        playerController.LoadPlayerConfig(gameManager.playerConfig);
        playerController.BuidDeck();

        enemyManager.LoadEnemy(gameManager.enemies);

       

        turnManager.SetTurnOnRound(2);
        turnManager.Initialze();
        turnManager.NextRound();

    }

    public void BattleClear()
    {
        //UIManager.Instance.CloseAll();
        playerController.ClearState();
        enemyManager.ClearState();
    }

    private void EndBattle(string message, bool isWin)
    {
        battleText.text = message;

        if (isWin)
        {
            //UIManager.Instance.OnWin();
            OnBattleWin?.Invoke();

        }
        else
        {
            //UIManager.Instance.OnLose();
            OnBattleLose?.Invoke();
        }
        BattleClear();

    }
    #endregion

    #region Event Handlers
    private void HandleEnemyEndTurn()
    {
        Debug.Log("Enemy End Turn");
        turnManager.NextRound();
    }

    private void HandlePlayerEndTurn()
    {
        Debug.Log("Player End Turn");
        enemyManager.Action();
    }

    private void HandleGameWin()
    {
        var result = new BattleResult(
        true,
        gold: 50,
        exp: 10
        );
        GameManager.Instance.BattleResult = result;

        //OnBattleFinished?.Invoke(result);
        EndBattle("Battle Win!", true);
    }

    private void HandleGameLose()
    {
        EndBattle("Battle Lose!", false);
    }
    #endregion

}
