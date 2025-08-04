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
    }

    private void Start()
    {
        healthCtrl = new Health(_health);
        _health.Initialize(100f, 0f);
        healthCtrl.OnDamageTaken.Subscribe(x => OnDameTaken()).AddTo(_disposables);
        healthCtrl.OnDeath.Subscribe(async x => await OnDeath()).AddTo(_disposables);

        if (_animator != null)
        {
            _animator.enabled = true;
        }
        IsDead.Value = false;

        counter.Initialize(
            initialCount: 1,
            initialMaxCount: 3,
            onCurrentCountChanged: () => HandleCurrentCounterChanged(),
            onCountReachedZero: () => HandleCountReachZero(),
            onMaxCountChanged: () => HandleMaxCountChanged()
        );
    }

    public void Count() => counter.DecreaseCount();
    private void HandleMaxCountChanged()
    {
        // animation or other logic when max count changes
    }

    private void HandleCountReachZero()
    {
        PlayerController.Instance.PlayerHealth.TakeDamage(10, DamageType.Physical);
        Debug.Log("Enemy has reached zero count.");
        counter.ResetCount();
    }
    public void Attack()
    {
        
    }
    private void HandleCurrentCounterChanged()
    {
        // animation or other logic when current count changes
    }

    private async UniTask OnDeath()
    {
        IsDead.Value = true;
        if (_animator != null)
        {
            _animator.SetBool("Dead", IsDead.Value);
        }

        try
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

            if (_animator != null)
            {
                _animator.enabled = false;
            }
            GetComponentInChildren<Image>().color = Color.black;
            if (EnemyManager.Instance.AllEnimiesDied())
                BattleManager.Instance.WinBattle();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Enemy destroyed before death animation finished");
        }
    }

    public void OnDameTaken()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Hurt");
        }
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