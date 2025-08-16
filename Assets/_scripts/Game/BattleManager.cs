using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;


public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CardManager deckManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private HandController handController;
    [SerializeField] private TextMeshProUGUI textPhase;
    private int _turnIndex;
    private async void Start()
    {
        Debug.Log("Player start");
        await deckManager.BattleStart();
        await handController.BattleStart();
        await enemyManager.BattleStart();
        await playerController.BattleStart();

        _turnIndex = 0;
        await PlayerTurn();
    }
    public async UniTask PlayerTurn()
    {
        textPhase.text = "WAIT FOR PLAYER ACTION";
        await handController.PlayerAction();
    }
    public async UniTask EnemyTurn()
    {
        textPhase.text = "ENEMY TURN";
        await enemyManager.EnemyTurn();
    }
    public void PauseGame()
    {
        textPhase.text = "WAIT FOR PAUSE";
    }
    
    public async UniTask GameWin()
    {
        textPhase.text = "PLAYER WIN";
        await ReturnToMap();
    }
    public async UniTask GameLose()
    {
        textPhase.text = "PLAYER LOSE";
        await ReturnToMap();
    }

    private async UniTask ReturnToMap()
    {
        await SceneManager.LoadSceneAsync("Map_Demo");
    }
    public async UniTask CheckCondition()
    {
        if (playerController.IsDead)
        {
            await GameLose();
        } else
        if (enemyManager.AllEnimiesDied())
        {
            await GameWin(); 
        } 

        _turnIndex++;
        switch (_turnIndex % 2)
        {
            case 0:
                await PlayerTurn();
                break;
            case 1:
                await EnemyTurn(); 
                break;
        }
    }
}