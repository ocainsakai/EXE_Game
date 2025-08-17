using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase enemiesSelected;
    [SerializeField] private CoinRSO playerCoin;
    [SerializeField] private EnemyBattleUI battleUI;
    [SerializeField] public List<Enemy> enemiesRuntime = new();
    public void InitializeEnemies()
    {
        enemiesRuntime = battleUI.InitializedEnemy(enemiesSelected.Enemies);
    }
    public Enemy GetTargetEnemy()
    {
        Debug.Log(enemiesRuntime.Count);
        return enemiesRuntime.FirstOrDefault(x => x.IsAlive);
    }
    public bool AllEnimiesDied()
    {
        return enemiesRuntime.TrueForAll(x => !x.IsAlive);
    }
    public async UniTask BattleStart()
    {
        InitializeEnemies();
        await UniTask.CompletedTask;
    }
    public async UniTask EnemyTurn()
    {
        foreach (var enemy in enemiesRuntime)
        {
            await enemy.Action();
            await UniTask.Delay(300);
        }
        await UniTask.Delay(1000);
        await BattleManager.Instance.CheckCondition();
    }
    public void GameWin()
    {
        Debug.Log("Gain Reward");
        int reward = 0;
        enemiesSelected.Enemies.ForEach(x => reward += x.Data.reward);
        playerCoin.onwnerCoins.Value += reward;
        enemiesSelected.Clear();
    }

    public void GameLose()
    {
        enemiesSelected.Clear();
    }
}
