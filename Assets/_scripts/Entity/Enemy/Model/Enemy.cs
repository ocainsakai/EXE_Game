using Ain;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDisposable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private HealthComponent _health;
    [SerializeField] private Counter counter;
    [SerializeField] private CounterDisplay counterDisplay;

    public EnemyState State => _sm.CurrentTypeState;
    private EnemyStateMachine _sm;
    private CompositeDisposable _disposables = new CompositeDisposable();
    public Health healthCtrl;
    public ReactiveProperty<bool> IsDead = new ReactiveProperty<bool>(false);

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        }
        if (_health == null)
        {
            _health = GetComponent<HealthComponent>();
        }
        _sm = new(this, _animator, counter);
    }

    private void Start()
    {

        healthCtrl = new Health(_health);
        _health.Initialize(100f, 0f);

        if (_animator != null)
        {
            _animator.enabled = true;
        }
        IsDead.Value = false;

        counter.Initialize(
            initialCount: 1,
            initialMaxCount: 3,
            onCountReachedZero: () => HandleCountReachZero()
        );
        _sm.ChangeState(EnemyState.Idle);
    }

    public void Count()
    {
        _sm.ChangeState(EnemyState.Counting);
    }
    private void HandleCountReachZero()
    {
        counter.ResetCount();
        Attack();
    }
    public void Attack()
    {
        _sm.ChangeState(EnemyState.Attack);

    }
  
    public void OnDeath()
    {
        
        _sm.ChangeState(EnemyState.Dead);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead.Value) return;
        healthCtrl.TakeDamage(damage, DamageType.Physical);
        _sm.ChangeState(EnemyState.Hurt);

    }
    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        _disposables?.Dispose();
        IsDead?.Dispose();
    }
    
}