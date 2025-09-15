using Map;
using UnityEngine;
using VContainer;

public class RunManager : MonoBehaviour
{
    [Inject]
    private PlayerManager playerManager;
    [Inject]    
    private BattleManager battleManager;
    [Inject]
    private EnemyManager gameManager;
    [Inject]
    private MapManager mapManager;
    public void InitNew()
    {
        mapManager.InitNew();
    }
}
