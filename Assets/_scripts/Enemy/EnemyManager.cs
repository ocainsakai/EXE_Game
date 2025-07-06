using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
public class EnemyManager : MonoBehaviour
{
    public GamePhase gamePhase;
    public List<Enemy> enemies = new List<Enemy>();
    public int targetesEnemy;
    private void Awake()
    {
        gamePhase.currentPhase.Subscribe(
           x =>
           {
               if (x == Phase.EnemiesTurn)
               {
                   enemies.ForEach(x => x.OnTurn());
               }
           });
        EnemiesSetup();
    }

    private void EnemiesSetup()
    {
        Enemy.EnemyDead.Subscribe(enemy => enemies.Remove(enemy));
        enemies = GetComponentsInChildren<Enemy>().ToList();
        targetesEnemy = 0;
    }

    public Enemy GetEnemy()
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.Log("next way or win");
            return null;
        }
        if (targetesEnemy >= 0 && targetesEnemy < enemies.Count && enemies[targetesEnemy] != null)
        {
            return enemies[targetesEnemy];
        }
        return enemies.FirstOrDefault();
    }
}
