using System;
using UnityEngine;

public class BattleEnemy : MonoBehaviour, ICanTakeDamege
{
    [SerializeField] EnemyData startingEnemyData;
    [SerializeField] EnemyAction enemyAction;
    [SerializeField] Health health;
    public EnemyState enemyState {  get; private set; }
    public Health Health => health;

    public Action<Enemy> onEnemyChanged;
    public Action onEnemyEndTurn;
    private Enemy _enemy;
    public Enemy enemy
    {
        get => _enemy;
        set
        {
            if (value != null && value != _enemy)
            {
                _enemy = value;
                onEnemyChanged?.Invoke(value);
            }
        }
    }

    public void LoadEnemy(EnemyData enemyData)
    {
        _enemy = new Enemy(enemyData ?? startingEnemyData);
        health.Init(enemyData.HP, enemyData.HP);
        enemyAction.MaxCount = enemyData.Count;
        enemyAction.currentCount = enemyData.Count;
    }

    private void OnEnable()
    {
        onEnemyChanged += UpdateEnemy;
    }

    private void OnDisable()
    {
        onEnemyChanged -= UpdateEnemy;
    }
    private void UpdateEnemy(Enemy enemy)
    {
        //health.Init(enemy.Data.HP, enemy.Data.HP);
    }
    public void TakeDamege(int damege)
    {
        health.TakeDamege(damege);
    }

    public void Heal(int heal)
    {
        health.Heal(heal);
    }
    public void Action()
    {
        enemyAction.CountToAction();
        onEnemyEndTurn?.Invoke();
    }

    public void ClearState()
    {

    }
}
