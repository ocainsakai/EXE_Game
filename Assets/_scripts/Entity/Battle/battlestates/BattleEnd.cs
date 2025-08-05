using Ain;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEnd : IState
{
    private readonly BattleManager _battleManager;
    private readonly bool _isVictory;
    private CancellationTokenSource _cts;
    private UniTask _battleResultTask;

    public BattleEnd(BattleManager battleManager, bool isVictory)
    {
        _battleManager = battleManager;
        _isVictory = isVictory;
    }

    public void OnEnter()
    {
        Debug.Log("BattleEnd.OnEnter() called");
        _cts = new CancellationTokenSource();
        _battleResultTask = HandleBattleResultAsync(_cts.Token);
    }

    public async void OnExit()
    {
        Debug.Log("BattleEnd.OnExit() called");

        // Cancel ongoing operations
        _cts?.Cancel();

        try
        {
            // Wait for the task to complete or be cancelled
            await _battleResultTask.SuppressCancellationThrow();
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
        }
    }

    public void Tick() { }

    private async UniTask HandleBattleResultAsync(CancellationToken ct)
    {
        try
        {
            // 1. Show battle result effects
            await ShowBattleResultEffects(ct);


            // 3. Return to map after delay
            await UniTask.Delay(500, cancellationToken: ct);

            if (!ct.IsCancellationRequested)
            {
                ReturnToMap();
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Battle result handling was cancelled");
            throw;
        }
    }

    private async UniTask ShowBattleResultEffects(CancellationToken ct)
    {
        if (_isVictory)
        {
            Debug.Log("You Win!");
            await ProcessVictoryRewards(ct);
        }
        else
        {
            Debug.Log("You Lose!");
        }
    }

    private async UniTask ProcessVictoryRewards(CancellationToken ct)
    {
        if (EnemyManager.Instance == null ||
            EnemyManager.Instance.enemiesInCombat == null ||
            _battleManager.playerController == null)
        {
            Debug.LogWarning("Missing references for victory rewards");
            return;
        }

        foreach (var enemy in EnemyManager.Instance.enemiesInCombat.Enemies)
        {
            if (enemy != null)
            {
                _battleManager.playerController.coinRSO.onwnerCoins.Value += enemy.reward;
                await UniTask.Yield(ct); // Allow cancellation between rewards
            }
        }
    }
    private void ReturnToMap()
    {
        try
        {
            _battleManager.Dispose();
            SceneManager.LoadScene("Map_Demo");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error returning to map: {e.Message}");
        }
    }
}