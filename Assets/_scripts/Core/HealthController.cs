using Ain;
using System;
using UniRx;

public class HealthController : IDisposable
{
    private HealthComponent _healthComponent;
    private CompositeDisposable _disposables = new CompositeDisposable();
    public float MaxHealth => _healthComponent.MaxHP.Value;

    public Subject<Unit> OnDamageTaken = new Subject<Unit>();
    public Subject<Unit> OnHealed = new Subject<Unit>();
    public Subject<Unit> OnDeath = new Subject<Unit>();
    public HealthController(HealthComponent healthComponent, float maxHP)
    {
        _healthComponent = healthComponent;
        _healthComponent.Initialize(maxHP, 0);
    }

    public void Dispose()
    {
        //throw new NotImplementedException();
    }
}