using Ain;
using Cysharp.Threading.Tasks;
using System.Threading;
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

        Debug.Log("BattleEnd.OnEnter() called");
        _cts = new CancellationTokenSource();
        // Xử lý kết thúc trận đấu (thắng/thua)
        HandleBattleResultAsync().Forget();
    }

    public void OnExit()
    {
        Debug.Log("BattleEnd.OnExit() called");
        // Hủy token để tránh rò rỉ bộ nhớ
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
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
            foreach (var enemy in EnemyManager.Instance.enemiesInCombat.Enemies)
            {
                _battleManager.playerController.coinRSO.onwnerCoins.Value += enemy.reward;
            }
        }
        else
        {
            Debug.Log("You Lose!");
          
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
