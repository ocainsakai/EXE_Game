using Map;
using UnityEngine;
using UnityUtils;
using VContainer;

public class RunManager : Singleton<RunManager>
{
    [Inject]
    private PlayerManager playerManager;
    [Inject]    
    private BattleManager battleManager;
    [Inject]
    private EnemyManager gameManager;
    [Inject]
    private MapManager mapManager;

    private void Start()
    {
        InitNew();
    }
    public void InitNew()
    {
        mapManager.InitNew();
    }
    public void Battle()
    {
        mapManager.Close();
        battleManager.InitNew();
    }
}
