using Ain;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEnd : IState
{
   
    private readonly BattleManager _battleManager;
    private readonly bool _isVictory; // Thắng hay thua?
    private CancellationTokenSource _cts;

    public BattleEnd(BattleManager battleManager, bool isVictory)
    {
        _battleManager = battleManager;
        _isVictory = isVictory;
        
    }

    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
        Debug.Log("BattleEnd.OnEnter() called");
        // Xử lý kết thúc trận đấu (thắng/thua)
        HandleBattleResultAsync().Forget();
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
    private async UniTaskVoid HandleBattleResultAsync()
    {
        // 1. Hiệu ứng kết thúc (ví dụ: màn hình chiến thắng/thua)
        if (_isVictory)
        {
            Debug.Log("You Win!");
            foreach (var enemy in _battleManager.enemyManager.enemiesInCombat.Enemies)
            {
                _battleManager.playerController.coinRSO.onwnerCoins.Value += enemy.reward;
            }
        }
        else
        {
            Debug.Log("You Lose!");
            foreach (var enemy in _battleManager.enemyManager.enemyList.enemies)
            {
                if (enemy.IsDead.Value)
                {
                    int coin = _battleManager.enemyManager.enemiesInCombat.Enemies
                        .Find(x => x.Prefab.GetComponent<Enemy>().name == enemy.name).reward;
                    _battleManager.playerController.coinRSO.onwnerCoins.Value += coin;
                }
                //Debug.Log("You Lose!");
                //await _battleManager.ShowDefeatScreen(_cts.Token);
            }
        }
        // 2. Dọn dẹp trận đấu
        _battleManager.ClearBattle();

        // 3. Chuyển về Map_Demo sau delay
        await UniTask.Delay(2000, cancellationToken: _cts.Token);
        ReturnToMap();
    }

    private void ReturnToMap()
    {
        // Đảm bảo hủy tất cả subscriptions trước khi chuyển scene
        _battleManager.Dispose();
        SceneManager.LoadScene("Map_Demo");
    }
}
