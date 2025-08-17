using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[SerializeField] private Animator _animator;
    [SerializeField] private Counter counter;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private EnemyData enemyData;

    public EnemyData Data => enemyData;
    public bool IsAlive => healthSystem.Health.Current.Value > 0;
    private bool CanAttack = false;

    private int AnimTime = 500;
    [SerializeField]  private int DeadTime = 1500;
    private void Awake()
    {
       
    }

    private void Start()
    {

        healthSystem.SetStat(new Stat(Data.HP));
        Debug.Log("is alive: " + IsAlive);
        counter.Initialize(
            initialCount: Data.Count,
            initialMaxCount: Data.Count,
            onCountReachedZero: () => CanAttack = true
        );
    }

    public async UniTask Action()
    {
        if ( !IsAlive)
        {
            return;
        }
        counter.DecreaseCount();
        await UniTask.Delay(counter.CountTime);
        if (CanAttack)
        {
            await Attack();
            CanAttack = false;
            counter.ResetCount();
        }
    }
    public async UniTask Attack()
    {
        await UniTask.Delay(AnimTime);
        await PlayerController.Instance.TakeDame(15);
    }

    public async UniTask TakeDamage(float damage)
    {
        if (!IsAlive) return;
        healthSystem.TakeDame((int) damage);

        int timer = Mathf.Max(AnimTime, healthSystem.AnimationDuration);
        await UniTask.Delay(timer);
        if (!IsAlive)
        {
            await OnDead();
        }
    }
     
    private async UniTask OnDead()
    {
        await UniTask.Delay(DeadTime);
        
    }
}