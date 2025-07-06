using TMPro;
using UniRx;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData Data;
    public EnemyStat enemyStat;
    public TextMeshPro enemyText;
    public static Subject<Enemy> EnemyDead = new Subject<Enemy>();
    [Header("Stat")]
    public ReactiveProperty<int> Health = new ReactiveProperty<int>();
    private void Awake()
    {
        enemyText = GetComponentInChildren<TextMeshPro>();
        enemyStat = GetComponentInChildren<EnemyStat>();
        Health.Subscribe(x => enemyText.text = $"{Health}/{Data.MaxHealth}");
    }
    private void Start()
    {
        Health.Value = Data.MaxHealth;
    }
    public void OnTurn()
    {
        enemyStat.CountToAction();
    }
    public void TakeDame(int dame)
    {
        Health.Value -= dame;
        Debug.Log(Health);
        if (Health.Value <= 0)
        {
            OnDead();
            EnemyDead.OnNext(this);
        }
    }
    public void OnDead()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
