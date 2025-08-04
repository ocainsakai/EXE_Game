using UnityEngine;

public class EnemyManager : MonoBehaviour
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
    public bool AllEnimiesDied()
    {
        return enemyList.enemies.TrueForAll(x => x.IsDead.Value);
    }
}
