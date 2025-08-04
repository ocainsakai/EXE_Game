using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] public EnemyList enemyList;
    [SerializeField] public EnemyDatabase enemiesInCombat;
    [SerializeField] public EnemyDatabase allEnemies;
    [SerializeField] public EnemyBattleUI battleUI;

    public void InitializeEnemies()
    {
        var enemies = battleUI.InitializedEnemy(enemiesInCombat.Enemies);
        enemyList.enemies.Clear();
        enemyList.enemies.AddRange(enemies);
    }
    public IEnumerable<Enemy> GetEnemiesAlive()
    {
        return enemyList.enemies.Where(x => !x.IsDead.Value);
    }
    public bool AllEnimiesDied()
    {
        return enemyList.enemies.TrueForAll(x => x.IsDead.Value);
    }
    public void EndTurn()
    {
        
    }
}
