using Map;
using UnityEngine;
using UnityUtils;
using VContainer;

public class RunManager : Singleton<RunManager>, IManager
{
    [Inject]
    private PlayerManager playerManager;
    [Inject]    
    private BattleManager battleManager;
    [Inject]
    private EnemyManager enemyManager;
    [Inject]
    private MapManager mapManager;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        mapManager.Init();
        playerManager.Init();
        enemyManager.Init();
        battleManager.Init();
    }
    public void StartBattle()
    {
        Hide();
        battleManager.Show();
        playerManager.Show();
        playerManager.StartRoom();
        enemyManager.Show();
    }
    public void Map()
    {
        Hide();
        mapManager.Show();
    }
    public void Hide()
    {
        mapManager.Hide();
        battleManager.Hide();
        enemyManager.Hide();
        playerManager.Hide();
    }

    public void Show()
    {
        mapManager.Show();
        battleManager.Show();
        enemyManager.Show();
        playerManager.Show();
    }
}
