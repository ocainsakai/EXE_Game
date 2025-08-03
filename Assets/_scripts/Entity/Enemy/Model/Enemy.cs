using Ain;
using UniRx;
using UnityEngine;

public class Enemy : GameTurner
{
    [SerializeField] Animator _animator;
    [SerializeField] HealthComponent _health;
    private Health health;

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
        health = new Health(_health);
        _health.Initialize(100f, 0f);
        _count = new ReactiveProperty<int>(1);
    }
    public void TakeDame(int amount) => health.TakeDamage(amount, DamageType.Physical);
}
