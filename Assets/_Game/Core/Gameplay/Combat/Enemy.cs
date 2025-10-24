using System.Collections;
using _Game.Core.Gameplay.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] private UIEnemyBattle uiEnemyBattle;
    [SerializeField] private BattleManager  battleManager;
    
    [HideInInspector]
    public EnemyData data;
    public Health health;
    public Count count;
    public UnityEvent<Enemy> onEndTurn;
    public void SetData(EnemyData data) {
        this.data = data;
        health = new(data.hp, data.hp);
        count = new(0, data.count);
        count.onFull.AddListener(Action);
        uiEnemyBattle.SetEnemy(this);
    }

    private void Action()
    {
        battleManager.AttackPlayer(data.atk);
        count.SetValue(0);
    }

    public void TakeDame(float dame)
    {
        health.Damage((int)dame);
    }

    public void HandleEnemyTurn()
    {
        count.CountUp();
        if (count.isFull)
        {
            Action();
        }
        EndEnemyTurn();
    }
    
    public void EndEnemyTurn()
    {
        battleManager.CheckCondition(this);
        onEndTurn?.Invoke(this);
    }
}
